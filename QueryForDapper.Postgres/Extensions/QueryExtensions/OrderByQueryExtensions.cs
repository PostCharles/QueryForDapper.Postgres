using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public static class OrderByQueryExtensions
    {
        public static IQuery OrderBy<T>(this IQuery query, Expression<Func<T, object>> propertySelector, Order order = default)
        {
            query.AddOrderBy(propertySelector.Body.GetMemberInfo(), typeof(T), order);

            return query;
        }
    }
}
