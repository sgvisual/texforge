using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using texforge_definitions.Settings;

namespace texforge.Graph.Nodes
{
    public enum eOperationType
    {
        None,
        Blur
    }

    class OperationType : Enumeration
    {
        public OperationType(string name, eOperationType value, eOperationType defaultValue)
            : base(name, value.ToString(), defaultValue.ToString(), typeof(eOperationType))
        {
            
        }

    }
}
