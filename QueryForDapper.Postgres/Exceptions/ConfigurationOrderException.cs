using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Exceptions
{
    public class ConfigurationOrderException : Exception
    {
        public const string ERROR_MESSAGE = "Naming methods must be setup prior to calling {0}";
        public ConfigurationOrderException(string method, Exception inner) : base(string.Format(ERROR_MESSAGE, method),inner)
        {

        }
    }
}
