using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Graph.Nodes
{
    public class Color : Node
    {
        texforge_definitions.Settings.Color color = new texforge_definitions.Settings.Color("Color", new texforge_definitions.Types.Color(System.Drawing.Color.White), new texforge_definitions.Types.Color(System.Drawing.Color.White),
            new texforge_definitions.Types.Color(System.Drawing.Color.Black), new texforge_definitions.Types.Color(System.Drawing.Color.White));

        Atom atom;

        public Color(string name, string id)
            : base(name, id)
        { }

        public override object Process()
        {
            if (atom == null)
                atom = new Atom(System.Drawing.Color.White);

            atom.AtomColor = color.Value.WindowsColor;

            // Unlike other nodes, this one doesnt need an atom,
            // it just needs to know the color and it will blended by the 
            // connected node, easiest way would be to have a special atom
            return atom;
        }

    }
}
