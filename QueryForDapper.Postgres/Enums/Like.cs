using QueryForDapper.Postgres.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Enums
{
    public enum Like
    {
        [SqlPartial("'%' || {0} || '%'")]
        Anywhere = 0,
        [SqlPartial("{0} || '%'")]
        Begins,
        [SqlPartial("'%' || {0}")]
        Ends
    }
}
