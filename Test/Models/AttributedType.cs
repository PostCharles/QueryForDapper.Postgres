using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Models
{

    [Table(AttributedType.TABLE_ATTRIBUTE_VALUE)]
    public class AttributedType
    {
        public const string COLUMN_ATTRIBUTE_VALUE = "column_id";
        public const string TABLE_ATTRIBUTE_VALUE = "table_name";

        [Column(COLUMN_ATTRIBUTE_VALUE)]
        public string id { get; set; }
    }
}
