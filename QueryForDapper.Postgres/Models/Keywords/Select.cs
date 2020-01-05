using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models.Keywords
{
    public class Select
    {
        public string Column { get;}
        public string Table { get; }
        public string As { get; }

        public Select(string column, string table, string @as) 
        {
            Column = column;
            Table = table;
            As = @as;
        }

        public string GetAsSqlPartial()
        {
            return (As is null) ? string.Empty : $" AS {As}";
        }
    }
}
