using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using QueryForDapper.Postgres.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace QueryForDapper.Postgres.Models
{
    public static class MiscQueryExtensions
    {
        public static IQuery SkipWith(this IQuery query, Expression<Func<object>> parameter)
        {
            query.AddOffsetParameter(parameter.Body.GetMemberInfo().Name);
            return query;
        }

        public static IQuery TakeWith(this IQuery query, Expression<Func<object>> parameter)
        {
            query.AddLimitParameter(parameter.Body.GetMemberInfo().Name);
            return query;
        }



    }
}
