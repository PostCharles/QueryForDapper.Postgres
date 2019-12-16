using QueryForDapper.Postgres.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Enums
{
    public enum Operator
    {
        [SqlPartial("")]
        NONE = 0,
        [SqlPartial("AND")]
        AND,
        [SqlPartial("OR")]
        OR,
        [SqlPartial("NOT")]
        NOT
    }
}
