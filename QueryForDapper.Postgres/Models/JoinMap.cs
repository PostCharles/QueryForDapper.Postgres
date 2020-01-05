using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Exceptions;
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

        private IDictionary<Type,string> ColumnsByTable { get; }
        private Type JoinTable { get; }


        public JoinMap(Type leftTable, Type joinTable, Type rightTable, string leftColumn, string rightColumn)
        {
            JoinTable = joinTable;
            
            ColumnsByTable = new Dictionary<Type, string>()
            {
                {leftTable, leftColumn },
                {rightTable, rightColumn }
            };

        }

        public bool CanJoin(Type leftTable, Type rightTable)
        {            
            return (ColumnsByTable.ContainsKey(leftTable) && 
                    ColumnsByTable.ContainsKey(rightTable));
        }

        public (string Column, Type Table) GetLeft(Type leftTable)
        {
            return (ColumnsByTable[leftTable], JoinTable);
        }

        public (string Column, Type Table) GetRight(Type rightTable)
        {
            return (ColumnsByTable[rightTable], rightTable);
        }
    }
}
