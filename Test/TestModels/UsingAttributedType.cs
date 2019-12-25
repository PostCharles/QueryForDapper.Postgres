using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.TestModels
{
    public class UsingAttributedType : DeclaringType
    {
        public const string COLUMN_ATTRIBUTE_NAME = "Attributed-Declared";
        [Column(COLUMN_ATTRIBUTE_NAME)]
        public override string Declared { get; set; }
    }
}
