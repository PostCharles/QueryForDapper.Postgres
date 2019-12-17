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
        inner = 0,
        [SqlPartial("LEFT OUTER")]
        LeftOuter,
        [SqlPartial("RIGHT OUTER")]
        RightOuter,
        [SqlPartial("FULL OUTER")]
        FullOuter

    }
}
