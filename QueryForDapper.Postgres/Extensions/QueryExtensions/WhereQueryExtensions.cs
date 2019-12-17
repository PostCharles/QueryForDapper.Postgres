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

        public static IQuery WhereCompared<T>(this IQuery query, Expression<Func<T, object>> propertySelector, string value,
                                              Operator @operator = default, Comparison comparison = default)
        {
            WhereComparedInternal<T>(query, propertySelector, $"'{value}'", @operator, comparison);

            return query;
        }

        public static IQuery WhereComparedWith<T>(this IQuery query, Expression<Func<T, object>> propertySelector, Expression<Func<object>> parameterSelector,
                                                  Operator @operator = default, Comparison comparison = default)
        {
            var parameter = $"@{parameterSelector.Body.GetMemberInfo().Name}";
            WhereComparedInternal<T>(query, propertySelector, parameter, @operator, comparison);

            return query;
        }

        private static void WhereComparedInternal<T>(IQuery query, Expression<Func<T, object>> propertySelector, string value, 
                                                     Operator @operator, Comparison comparison)
        {
            var column = propertySelector.Body.GetMemberInfo();
            query.AddWhere(column, typeof(T), $"{comparison.GetSql()} {value}", @operator);
        }


        public static IQuery WhereLike<T>(this IQuery query, Expression<Func<T, object>> propertySelector, string value, 
                                          Operator @operator = default, Case likeCase = default, Like like = default)
        {
            WhereLikeInternal(query, propertySelector, $"'{value}'", @operator, like, likeCase);
            
            return query;
        }

        public static IQuery WhereLikeWith<T>(this IQuery query, Expression<Func<T, object>> propertySelector, Expression<Func<object>> parameterSelector,
                                              Operator @operator = default, Like like = default, Case @case = default)
        {
            var parameter = parameterSelector.Body.GetDapperParameter();
            WhereLikeInternal(query, propertySelector, parameter, @operator, like, @case);

            return query;
        }

        private static void WhereLikeInternal<T>( IQuery query, Expression<Func<T, object>> propertySelector, string likeValue, Operator @operator, Like like, Case @case)
        {
            var column = propertySelector.Body.GetMemberInfo();
            var predicate = $"{@case.GetSql()} {String.Format(like.GetSql(), likeValue)}";

            query.AddWhere(column, typeof(T), predicate, @operator);
        }

        public static IQuery WhereAnyWith<T>(this IQuery query, Expression<Func<T, object>> propertySelector, Expression<Func<object>> parameterSelector, Operator @operator = default)
        {
            var dapperParameter = parameterSelector.Body.GetDapperParameter();
            var column = propertySelector.Body.GetMemberInfo();

            var predicate = $"= {ANY}( {dapperParameter} )";
            query.AddWhere(column, typeof(T), predicate, @operator);

            return query;
        }

        public static IQuery WhereInSubQuery<T>(this IQuery query, Expression<Func<T, object>> propertySelector, IQuery subQuery, Operator @operator = default )
        {
            query.AddWhere(propertySelector.GetMemberInfo(), typeof(T), $"IN ({subQuery.ToStatement()})", @operator);

            return query;
        } 
    }
}
