﻿using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public static class SelectQueryExtensions
    {
        public const string SELECT_ALL = "*";

        public static IQuery Select<T>(this IQuery query)
        {
            query.AddSelect(SELECT_ALL, typeof(T));

            return query;
        }

        public static IQuery Select<T>(this IQuery query, string column)
        {
            query.AddSelect(column, typeof(T));

            return query;
        }

        public static IQuery Select<T>(this IQuery query, params string[] columns)
        {
            foreach (var column in columns)
            {
                query.AddSelect(column, typeof(T));
            }

            return query;
        }

        public static IQuery Select<T>(this IQuery query, Expression<Func<T, object>> propertySelector)
        {
            query.AddSelect(propertySelector.Body.GetMemberInfo(), typeof(T));

            return query;
        }

        public static IQuery Select<T>(this IQuery query, params Expression<Func<T, object>>[] propertySelectors)
        {
            foreach (var propertySelector in propertySelectors)
            {
                query.AddSelect(propertySelector.Body.GetMemberInfo(), typeof(T));
            }

            return query;
        }

        public static IQuery SelectAs<T>(this IQuery query, string column, string @as)
        {
            query.AddSelect(column, typeof(T), @as);

            return query;
        }

        public static IQuery SelectAs<T>(this IQuery query, Expression<Func<T, object>> propertySelector, string @as)
        {
            query.AddSelect(propertySelector.Body.GetMemberInfo(), typeof(T), @as);

            return query;
        }
    }
}
