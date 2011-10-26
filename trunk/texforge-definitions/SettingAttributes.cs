using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge_definitions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class SettingAttributes : Attribute
    {        

        protected bool canRandomize = true;
        public bool CanRandomize
        {
            get { return canRandomize; }
            set { canRandomize = value; }
        }

        protected bool useLimits = false;
        public bool UseLimits
        {
            get { return useLimits; }
            set { useLimits = value; }
        }

    }
}
