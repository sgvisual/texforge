using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace texforge.Base
{
    public class UniqueName
    {
        protected string name;
        public UniqueName(string id)
        {
            Guid guid = Guid.NewGuid();
            if (id.Length == 0)
            {
                name = guid.ToString();
            }
            else
            {
                name = id;
            }
                
        }

        public string Value
        {
            get { return name; }
        }
    }
}
