using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public static class JoinQueryExtensions
    {
        public static IQuery JoinOn<T>(this IQuery query, string column, JoinType joinType = default)
        {
            query.AddJoin(column, typeof(T), joinType);

            return query;
        }
        public static IQuery JoinOn<T>(this IQuery query, Expression<Func<T, object>> fieldSelector, JoinType joinType = default)
        {
            query.AddJoin(fieldSelector.Body.GetMemberInfo(), typeof(T), joinType);

            return query;
        }

        public static IQuery JoinMany<TLeft, TRight>(this IQuery query)
        {
            var joinMap = QueryConfiguration.Current.JoinMaps.SingleOrDefault(j => j.LeftTable == typeof(TLeft) && j.RightTable == typeof(TRight));

            if (joinMap is null) throw new JoinMapNotFound(typeof(TLeft), typeof(TRight));

            query.AddJoin(joinMap.LeftKey, joinMap.JoinTable, JoinType.INNER);
            query.AddJoin(joinMap.RightKey, joinMap.RightTable, JoinType.INNER);

            return query;
        }
    }
}
