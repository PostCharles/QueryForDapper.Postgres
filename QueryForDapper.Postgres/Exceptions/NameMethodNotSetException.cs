using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Exceptions
{
    public class NameMethodNotSetException : Exception
    {
        public NameMethodNotSetException(string propertyName) : base($"A naming method must be configured for {propertyName.Replace("Namer", "")}s. " +
                                                                     $"See configuration methods under {nameof(Query)}.{nameof(Query.ConfigureTo)}")
        {
        }
    }
}
