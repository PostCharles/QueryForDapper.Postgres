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
        public bool IsUsing { get; }
        public string LeftColumn { get; set; }
        public string LeftTable { get; set; }

        public Join(string column, string table, JoinType joinType)
        {
            Column = column;
            Table = table;

            IsUsing = true;

            JoinType = joinType.GetSql();
        }

        public Join(string column, string table, string leftColumn, string leftTable, JoinType joinType)
        {
            Column = column;
            Table = table;
            LeftColumn = leftColumn;
            LeftTable = leftTable;

            IsUsing = false;
            JoinType = joinType.GetSql();
        }

    }
}
