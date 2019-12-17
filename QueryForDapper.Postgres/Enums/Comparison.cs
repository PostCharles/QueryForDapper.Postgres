using QueryForDapper.Postgres.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Enums
{
    public enum Comparison
    {
        [SqlPartial("=")]
        Equal = 0,
        [SqlPartial("<>")]
        NotEqual,
        [SqlPartial("<")]
        LessThan,
        [SqlPartial("<=")]
        LessThanEqual,
        [SqlPartial(">")]
        GreaterThan,
        [SqlPartial(">=")]
        GreaterThanEqual

    }
}
