using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;
using System.Windows.Forms;
using texforge_definitions;
using System.Drawing;
using texforge.Graph;

namespace texforge
{
    public class SettingComponentFactory
    {
        public static void CreateComponent(SettingBase setting, VisualGraph.DraggableObject owner, FlowLayoutPanel panel)
        {
            SettingComponent component = null;
            switch (setting.GetType().Name)
            {
                case "Color":
                    component = new ColorSettingComponent(setting, owner);
                    break;
                default:
                    component = new InvalidSettingComponent(setting, owner);
                    break;
            }
            panel.Controls.Add(component.Container);
        }

        abstract class SettingComponent
        {
            protected Panel container;
            protected VisualGraph.DraggableObject owner;
            public Panel Container
            {
                get { return container; }
            }

            public SettingComponent(VisualGraph.DraggableObject owner)
            {
                container = new Panel();
                container.Dock = DockStyle.Bottom;
                container.Tag = this;
                this.owner = owner;
            }

            protected void ValueChanged()
            {
                owner.Dirty = true;
            }
        }

        class InvalidSettingComponent : SettingComponent
        {
            public InvalidSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner)
                : base(owner)
            {
                Label invalid = new Label();
                invalid.Text = "Unsupported type: " + setting.GetType().Name;
                invalid.Dock = DockStyle.Fill;
                container.Controls.Add(invalid);
            }
        }
         
        class ColorSettingComponent : SettingComponent
        {
            texforge_definitions.Settings.Color data;

            public ColorSettingComponent(SettingBase setting, VisualGraph.DraggableObject owner)
                : base(owner)
            {
                data = (texforge_definitions.Settings.Color)setting;
                GroupBox box = new GroupBox();
                box.Text = data.Name;
                box.Dock = DockStyle.Fill;
                SplitContainer panel = new SplitContainer();
                PictureBox color = new PictureBox();
                panel.Panel1.Controls.Add(color);
                Button change = new Button();
                change.Text = "Change";
                panel.Panel2.Controls.Add(change);
                panel.Dock = DockStyle.Fill;
                box.Controls.Add(panel);
                container.Controls.Add(box);
                color.Paint += new PaintEventHandler(color_Paint);
            }

            void color_Paint(object sender, PaintEventArgs e)
            {
                data.Randomize();
                ValueChanged();
                e.Graphics.FillRectangle(new SolidBrush(data.Value.WindowsColor), e.ClipRectangle);
            }

        }

    }

}
