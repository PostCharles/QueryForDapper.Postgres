using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Models
{
    public class PropertyTest
    {
        public int IntName { get; set; }
        public byte ByteName { get; set; }
        public decimal DecimalName { get; set; }
        public double DoubleName { get; set; }
        public string StringName { get; set; }
        public bool BoolName { get; set; }
        public DateTime DateTimeName { get; set; }
        public NestedClass ClassName { get; set; }

        public class NestedClass
        {

        }
    }

    
}
