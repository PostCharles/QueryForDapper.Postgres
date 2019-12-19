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
        public static IQuery JoinOn<T>(this IQuery query, Expression<Func<T, object>> propertySelector, JoinType joinType = default)
        {
            query.AddJoin(propertySelector.Body.GetMemberInfo(), typeof(T), joinType);

            return query;
        }

        public static IQuery JoinOn<TLeft, TRight>(this IQuery query, string columnLeft, string columnRight, JoinType joinType = default)
        {
            query.AddJoin(columnRight, typeof(TRight), columnLeft, typeof(TLeft), joinType);

            return query;
        }

        public static IQuery JoinOn<TLeft, TRight>(this IQuery query, Expression<Func<TLeft, object>> leftSelector, 
                                                   Expression<Func<TRight, object>> rightSelector, JoinType joinType = default)
        {
            query.AddJoin(rightSelector.GetMemberInfo(), typeof(TRight), leftSelector.GetMemberInfo(), typeof(TLeft), joinType);

            return query;
        }

        public static IQuery JoinMany<TLeft, TRight>(this IQuery query, JoinType joinType = default)
        {
            var leftType = typeof(TLeft);
            var rightType = typeof(TRight);

            if (leftType == rightType) throw new JoinManyLeftRightEqualException(leftType);

            var joinMap = QueryConfiguration.Current.JoinMaps.FirstOrDefault(m => m.CanJoin(leftType, rightType));
            if (joinMap is null) throw new JoinMapNotFound(leftType, rightType);

            var left = joinMap.GetLeft(leftType);
            query.AddJoin(left.Column, left.Table, joinType);
            
            var right = joinMap.GetRight(rightType);
            query.AddJoin(right.Column, right.Table, joinType);

            return query;
        }
    }
}
