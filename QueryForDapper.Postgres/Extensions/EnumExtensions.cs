using QueryForDapper.Postgres.Attributes;
using QueryForDapper.Postgres.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Extensions
{
    public static class EnumExtensions
    {
        public const string ERROR_MESSAGE = "{0} is missing on enum value {1}.{2}";
        public static string GetSql<T>(this T @enum ) where T: Enum
        {
            Type enumType = @enum.GetType();

            var member = enumType.GetMember(enumType.GetEnumName(@enum))[0];

            var attribute = member.GetCustomAttributes(typeof(SqlPartialAttribute) ,inherit:false).FirstOrDefault();

            if (attribute is null) throw new MissingSqlPartialException(@enum);

            return ((SqlPartialAttribute)attribute).Sql;

        }
    }
}
