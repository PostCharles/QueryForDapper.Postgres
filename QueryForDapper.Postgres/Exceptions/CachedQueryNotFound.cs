using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Exceptions
{
    public class CachedQueryNotFound : Exception
    {
        public const string ERROR_MESSAGE = "Unable to find query using id {0}";

        public const string ERROR_MESSAGE_TYPE = "Unable to find query using id {0} and type {1}";

        public CachedQueryNotFound(string id) : base(String.Format(ERROR_MESSAGE, id))
        {

        }

        public CachedQueryNotFound(string id, Type type) : base(String.Format(ERROR_MESSAGE_TYPE, id, type.Name))
        {

        }
    }
}
