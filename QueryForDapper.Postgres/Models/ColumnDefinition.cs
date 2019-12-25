using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public bool IsMatch(string column, Type table)
        {
            return (column == PropertyName && table == TableType);
        }
    }
}
