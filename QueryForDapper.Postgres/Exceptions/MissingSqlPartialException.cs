using QueryForDapper.Postgres.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Exceptions
{
    public class MissingSqlPartialException : Exception
    {
        public const string ERROR_MESSAGE = "{0} is missing on enum value {1}.{2}";

        public MissingSqlPartialException(Enum @enum) : base(String.Format(ERROR_MESSAGE, nameof(SqlPartialAttribute), @enum.GetType().Name, @enum.ToString())) { }
    }
}
