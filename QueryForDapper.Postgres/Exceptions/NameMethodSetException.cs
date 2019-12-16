using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Exceptions
{
    public class NameMethodSetException : Exception
    {
        public NameMethodSetException(string propertyName) : base($"Query nameing for {propertyName.Replace("Namer","")}s has already been configured")
        { 
        }

    }
}
