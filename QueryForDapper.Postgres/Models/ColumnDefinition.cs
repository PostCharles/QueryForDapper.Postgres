using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public class ColumnDefinition
    {
        public Type TableType { get; set; }
        public string PropertyName { get; set; }
        public string ColumnName { get; set; }

        public ColumnDefinition(Type tableType, string propertyName, string columnName)
        {
            TableType = tableType;
            PropertyName = propertyName;
            ColumnName = columnName;
        }
    }
}
