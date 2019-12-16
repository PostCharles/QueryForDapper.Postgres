using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models.Keywords
{
    public class Where
    {
        public string Column { get; }
        public string Table { get; }
        public string Operator { get; }
        public string Predicate { get; }

        public Where(string column, string table, Operator @operator, string predicate)
        {
            Column = column;
            Table = table;
            Operator = @operator.GetSql();
            Predicate = predicate;
        }

    }
}
