using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public class JoinMap
    {
        public string LeftKey { get; }
        public Type LeftTable { get; }
        public Type JoinTable { get; }
        public Type RightTable { get; }
        public string RightKey { get; }

        public JoinMap(Type leftTable, Type joinTable, Type rightTable, string leftKey, string rightKey)
        {
            LeftTable = leftTable;
            JoinTable = joinTable;
            RightTable = rightTable;
            LeftKey = leftKey;
            RightKey = rightKey;
        }


    }
}
