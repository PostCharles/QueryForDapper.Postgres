using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Exceptions
{
    public class JoinManyLeftRightEqualException : Exception
    {
        public const string ERRROR_MESSAGE = "The type {0} was passed for both the Left and Right table";
        public JoinManyLeftRightEqualException(Type tableType) : base(String.Format(ERRROR_MESSAGE, tableType.Name)) { }
    }
}
