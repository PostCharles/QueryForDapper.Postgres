using QueryForDapper.Postgres.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models.Keywords
{
    public class OrderBy
    {
        public string Column { get; }
        public string Table { get; }
        public string Order { get; }

        public OrderBy(string column, string table, Order order)
        {
            Column = column;
            Table = table;
            Order = order.ToString();
        }

    }
}
