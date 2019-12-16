using QueryForDapper.Postgres.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Enums
{
    public enum Case
    {
        [SqlPartial("ILIKE")]
        Insensitive = 0,
        [SqlPartial("LIKE")]
        Sensitive
    }
}
