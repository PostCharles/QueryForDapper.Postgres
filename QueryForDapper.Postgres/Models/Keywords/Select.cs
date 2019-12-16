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

        public Select(string column, string table) 
        {
            Column = column;
            Table = table;
        }
    }
}
