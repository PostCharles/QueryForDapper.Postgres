using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Exceptions
{
    public class JoinMapNotFound : Exception
    {
        public const string ERROR_MESSAGE = "A join map doesn't exist for types {0} and {1}. Define maps with Query.ConfigureTo()";
        public JoinMapNotFound(Type leftTable, Type rightTable) : base(String.Format(ERROR_MESSAGE,leftTable.Name, rightTable.Name))
        {

        }
    }
}
