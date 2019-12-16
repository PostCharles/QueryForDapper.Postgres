using QueryForDapper.Postgres.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Enums
{
    public enum JoinType
    {
        [SqlPartial("INNER")]
        INNER = 0,
        [SqlPartial("LEFT OUTER")]
        LEFT_OUTER,
        [SqlPartial("RIGHT OUTER")]
        RIGHT_OUTER,
        [SqlPartial("FULL OUTER")]
        FULL_OUTER

    }
}
