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
        [SqlPartial("AND")]
        And = 0,
        [SqlPartial("OR")]
        Or,
        [SqlPartial("NOT")]
        Not
    }
}
