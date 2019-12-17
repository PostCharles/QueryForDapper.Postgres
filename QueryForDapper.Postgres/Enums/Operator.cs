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
        None = 0,
        [SqlPartial("AND")]
        And,
        [SqlPartial("OR")]
        Or,
        [SqlPartial("NOT")]
        Not
    }
}
