using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Models
{
    public class AttributedType
    {
        public const string COLUMN_ATTRIBUTE_VALUE = "column_id";

        [Column(COLUMN_ATTRIBUTE_VALUE)]
        public string id { get; set; }
    }
}
