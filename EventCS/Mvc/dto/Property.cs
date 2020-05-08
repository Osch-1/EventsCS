using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.dto
{
    public class Property
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }

        public Property()
        {

        }
        public Property(string name, string value, string type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
        }
    }
}
