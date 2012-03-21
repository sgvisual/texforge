using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using texforge.Graph;
using texforge_definitions.Settings;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace texforge
{
    public abstract class VisualGraph
    {
        public class ExportOptions
        {
            public Size resolution;
            public int nbFrames;
            public enum ExportType
            {
                SingleImage,
                MultipleImages,
            }
            public ExportType type;
            public int tileX;
            public int tileY;
        }

        Point offset = new Point();
        protected float zoom = 100.0f;
        protected Thread processing = null;
        protected bool graphPossiblyDirty = false;
        protected Graph.Graph.SharedThreadProperties sharedThreadProperties = new Graph.Graph.SharedThreadProperties();

        int currentFrame = -1;

        protected DraggableObject dragging = null;
        DraggableObject active = null;
        public DraggableObject ActiveObject
        {
            set { active = value; }
            get { return active; }
        }

        protected Graph.Graph graph;
        public Graph.Graph Graph
        {
            get { return graph; }
        }

        Preview currentPreview = null;
        public Preview CurrentPreview
        {
            set { currentPreview = value; }
            get { return currentPreview; }
        }

        public int TotalNodes
        {
            get { return graph.Nodes.Count; }
        }

        protected int dirtyNodes = 0;
        public int DirtyNodes
        {
            get { return dirtyNodes; }
        }

        protected bool debug = false;

        string associatedFile = "";
        public string AssociatedFile
        {
            get { return associatedFile; }
        }

        bool modified = false;
        public bool Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        public Image Output
        {
            get
            {
                Node final = graph.ProceduralFinal;
                if (graph.FinalOutput.Count > 0)
                {
                    // Start of the animation
                    if (currentFrame < 0 || currentFrame >= graph.FinalOutput.Count)
                    {
                        currentFrame = 0;
                    }
                    final = graph.FinalOutput[currentFrame];
                }
                if (final != null && final.DisplayAtom != null && final.DisplayAtom.Result != null)
                {
                    return final.DisplayAtom.Result;
                }
                return null;
            }
        }

        public void AdvanceAnimationFrame()
        {
            if (++currentFrame >= graph.FinalOutput.Count)
            {
                currentFrame = 0;
            }
        }

        public void AddFinalOutput(Node node)
        {
            graph.FinalOutput.Add(node);
        }

        public void RemoveFinalOutput(Node node)
        {
            graph.FinalOutput.Remove(node);
        }

        public bool FinalOutputContains(Node node)
        {
            return graph.FinalOutput.Contains(node);
        }


        // Cached data
        Dictionary<Graph.Node.Socket, Rectangle> cachedSocketRender = new Dictionary<Node.Socket, Rectangle>();
        public Dictionary<Graph.Node.Socket, Rectangle> CachedSocketRender
        {
            get { return cachedSocketRender; }
        }


        public VisualGraph()
        {
            graph = new UnitTest_Graph().graph;
        }

        public bool Debug
        {
            get { return debug; }
            set { debug = value; }
        }

        public virtual void Clear()
        {
            graph = new Graph.Graph(new GraphSettings());
            associatedFile = "";
            modified = false;
            offset = new Point();
            zoom = 100.0f;
            dragging = null;
            active = null;
        }

        public virtual void Zoom(float amount)
        {
            zoom += amount;
            if (zoom < 30.0f)
                zoom = 30.0f;
            if (zoom > 300.0f)
                zoom = 300.0f;
        }

        public void Pan(Point delta)
        {
            offset.X += (int)((float)delta.X / zoom * 100.0f);
            offset.Y += (int)((float)delta.Y / zoom * 100.0f);
        }

        public virtual void Invalidate()
        {
        }

        public bool IsAnimated
        {
            get { return graph.FinalOutput.Count > 1; }
        }

        public abstract void AbortThread();

        protected void Process()
        {
            while (true)
            {
                if (graphPossiblyDirty)
                {
                    graphPossiblyDirty = false;
                    graph.Process(sharedThreadProperties);
                }
                if (sharedThreadProperties.invalidate)
                {
                    sharedThreadProperties.invalidate = false;
                    Invalidate();
                }
                Thread.Sleep(1);
            }
        }

        protected Point GetCenter(Rectangle clip)
        {
            return new Point((clip.Width - clip.X) / 2 + (int)((float)offset.X / 100.0f * zoom), (clip.Height - clip.Y) / 2 + (int)((float)offset.Y / 100.0f * zoom));
        }

        public Point TransformFromScreen(Point position, Rectangle clip)
        {
            Point center = GetCenter(clip);
            position.X -= center.X;
            position.X = (int)((float)position.X / (float)zoom * 100.0f);
            position.Y -= center.Y;
            position.Y = (int)((float)position.Y / (float)zoom * 100.0f);
            return position;
        }

        public Point TransformToScreen(Point position, Rectangle clip)
        {
            Point center = GetCenter(clip);
            position.X = (int)((float)position.X / 100.0f * (float)zoom);
            position.X += center.X;
            position.Y = (int)((float)position.Y / 100.0f * (float)zoom);
            position.Y += center.Y;
            return position;
        }

        public List<string> NodeTypes
        {
            get { return NodeFactory.Get().NodeTypes; }
        }

        public DraggableObject AddNode(string type, Point position, Rectangle currentClip)
        {
            Graph.Node a = graph.CreateNode(type, "");
            a.Name = type + graph.Nodes.Count;
            NodeData aData = new NodeData();
            a.Data = aData;
            a.Data.header.point = TransformFromScreen(position, currentClip);
            modified = true;
            return new DraggableNode(a, new Point());
        }
        public DraggableObject GetDraggableObject(Point position, Rectangle currentClip)
        {
            // Check for sockets
            foreach (KeyValuePair<Node.Socket, Rectangle> socket in cachedSocketRender)
            {
                if (position.X >= socket.Value.X && position.X < socket.Value.X + socket.Value.Width &&
                    position.Y >= socket.Value.Y && position.Y < socket.Value.Y + socket.Value.Height)
                {
                    return new DraggableSocket(socket.Key);
                }
            }
            // Check for nodes
            Point world = TransformFromScreen(position, currentClip);
            foreach (Graph.Node node in graph.Nodes)
            {
                DraggableObject drag = NodeGetDraggableObject(node, world);
                if (drag != null)
                    return drag;
            }
            return null;
        }

        DraggableObject NodeGetDraggableObject(Graph.Node node, Point atPosition)
        {
            Point position = node.Data.header.point;
            Size size = NodeGetSize(node);
            if (atPosition.X >= position.X && atPosition.Y >= position.Y &&
                atPosition.X < position.X + size.Width && atPosition.Y < position.Y + size.Height)
            {
                return new DraggableNode(node, new Point(atPosition.X - position.X, atPosition.Y - position.Y));
            }
            return null;
        }

        protected Size NodeGetSize(Graph.Node node)
        {
            const int minimumNodeWidth = 100;
            const int minimumNodeHeight = 75;
            const int connectionsHeight = 20;
            int extraConnections = Math.Max(node.InputSockets.Count, node.OutputSockets.Count) - 2;
            int nodeHeight = minimumNodeHeight;
            if (extraConnections > 0)
            {
                nodeHeight += connectionsHeight * extraConnections;
            }
            return new Size(minimumNodeWidth, nodeHeight);
        }

        public void DropDraggedObject(DraggableObject what, Point position, Rectangle currentClip)
        {
            dragging.Drop(this, position, currentClip);
            dragging = null;
        }

        public void DraggingObject(DraggableObject what, Point position, Rectangle currentClip)
        {
            dragging = what;
            dragging.Position = TransformFromScreen(position, currentClip);
        }

        public void Save()
        {
            Save(associatedFile);
        }

        public void Save(string filename)
        {
            associatedFile = filename;
            graph.Save(filename);
            modified = false;
        }

        public void Load(string filename)
        {
            Clear();
            associatedFile = filename;
            graph.Load(filename);
        }

        public void ExportAs(string filename)
        {
            Output.Save(filename);
        }

        public void ExportAs(string filename, ExportOptions exportOptions)
        {
            switch (exportOptions.type)
            {
                case ExportOptions.ExportType.MultipleImages:
                    int countLength = exportOptions.nbFrames.ToString().Length;
                    string format = "{0:d" + countLength + "}";
                    string path = System.IO.Path.GetDirectoryName(filename);
                    string file = System.IO.Path.GetFileNameWithoutExtension(filename);
                    string extension = System.IO.Path.GetExtension(filename);
                    int i = 1;
                    foreach (Node output in graph.FinalOutput)
                    {
                        output.DisplayAtom.Result.Save(path + "\\" + file + "_" + string.Format(format, i++) + extension);
                    }
                    break;

                case ExportOptions.ExportType.SingleImage:
                    Bitmap final = new Bitmap(exportOptions.tileX * exportOptions.resolution.Width, exportOptions.tileY * exportOptions.resolution.Height);
                    int x = 0;
                    int y = 0;
                    foreach (Node output in graph.FinalOutput)
                    {
                        Graphics graphics = Graphics.FromImage(final);
                        graphics.DrawImage(output.DisplayAtom.Result, new Point(x * exportOptions.resolution.Width, y * exportOptions.resolution.Height));
                        if (++x >= exportOptions.tileX)
                        {
                            x = 0;
                            ++y;
                        }
                    }
                    final.Save(filename);
                    break;
            }
        }

    }
}
