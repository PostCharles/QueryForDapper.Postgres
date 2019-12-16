using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Extensions;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public static class WhereQueryExtensions
    {
        public const string ERROR_LIKE_SWITCH = "No handler for {0}.{1}";
        
        public const string ANY = "ANY";
        

        public static IQuery WhereAnyWith<T>(this IQuery query, Expression<Func<T, object>> fieldSelector, Expression<Func<object>> parameterSelector, Operator @operator = default)
        {
            var dapperParameter = parameterSelector.Body.GetDapperParameter();
            var column = fieldSelector.Body.GetMemberInfo();

            var predicate = $"= {ANY}( {dapperParameter} )";
            query.AddWhere(column, typeof(T), predicate, @operator);

            return query;
        }

        public static IQuery WhereLike<T>(this IQuery query, Expression<Func<T, object>> fieldSelector, string value, 
                                         Operator @operator = default, Case likeCase = default, Like like = default)
        {
            BuildLike(query, fieldSelector, $"'{value}'", @operator, like, likeCase);
            return query;
        }

        public static IQuery WhereLikeWith<T>(this IQuery query, Expression<Func<T, object>> fieldSelector, Expression<Func<object>> parameterSelector,
                                             Operator @operator = default, Like like = default, Case @case = default)
        {
            var parameter = parameterSelector.Body.GetDapperParameter();
            BuildLike(query, fieldSelector, parameter, @operator, like, @case);

            return query;
        }

        private static void BuildLike<T>( IQuery query, Expression<Func<T, object>> fieldSelector, string likeValue, Operator @operator, Like like, Case @case)
        {
            var column = fieldSelector.Body.GetMemberInfo();
            var predicate = $"{@case.GetSql()} {String.Format(like.GetSql(), likeValue)}";

            query.AddWhere(column, typeof(T), predicate, @operator);
        }

        public static IQuery WhereInSubQuery<T>(this IQuery query, Expression<Func<T, object>> fieldSelector, IQuery subQuery, Operator @operator = default )
        {
            query.AddWhere(fieldSelector.GetMemberInfo(), typeof(T), $"IN ({subQuery.ToStatement()})", @operator);

            return query;
        } 
    }
}
