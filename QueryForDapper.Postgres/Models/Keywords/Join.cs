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
    public class Join
    {
        public string Column { get; }
        public string Table { get; }
        public string JoinType { get; }

        public Join(string column, string table, JoinType joinType)
        {
            Column = column;
            Table = table;
            
            JoinType = joinType.GetSql();
        }

    }
}
