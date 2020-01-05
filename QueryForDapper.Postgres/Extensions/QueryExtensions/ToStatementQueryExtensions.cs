using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public static class ToStatementQueryExtensions
    {
        private const string SELECT = "SELECT";
        private const string ALL = "*";
        private const string EQUALS = "=";
        private const string FROM = "FROM";
        private const string JOIN = "JOIN";
        private const string USING = "USING";
        private const string ORDER_BY = "ORDER BY";
        private const string OFFSET = "OFFSET";
        private const string ON = "ON";
        private const string LIMIT = "LIMIT";
        private const string WHERE = "WHERE";

        public static string ToStatement(this IQuery query)
        {
            var statement = BuildSelect(query);

            statement = AppendJoins(query, statement);
            statement = AppendWheres(query, statement);
            statement = AppendOrderBys(query, statement);
            statement = AppendLimit(query, statement);
            statement = AppendOffset(query, statement);

            return statement;
        }

        public static string BuildSelect(IQuery query)
        {
            if (query.Selects.Count == 0) return $"{SELECT} {ALL} {FROM} {query.StartTable}";


            var result = $"{SELECT}";

            foreach (var select in query.Selects)
            {
                result = $"{result} {select.Table}.{select.Column}{select.GetAsSqlPartial()},";
            }

            return $"{result.TrimEnd(',')} {FROM} {query.StartTable}";
            
        }

        public static string AppendJoins(IQuery query, string sql)
        {
            foreach (var join in query.Joins)
            {
                sql = $"{sql} {join.JoinType} {JOIN} {join.Table}";

                if (join.IsUsing) sql = $"{sql} {USING} ({join.Column})";
                else sql = $"{sql} {ON} {join.LeftTable}.{join.LeftColumn} {EQUALS} {join.Table}.{join.Column}";
            }

            return sql;
        }

        public static string AppendWheres(IQuery query, string sql)
        {
            if (query.Wheres.Count == 0) return sql;

            sql = $"{sql} {WHERE}";

            foreach (var where in query.Wheres)
            {
                sql = $"{sql} {where.Operator} {where.Table}.{where.Column} {where.Predicate}";
            }

            return Regex.Replace(sql, @"\s+", " ");
        }

        public static string AppendOrderBys(IQuery query, string sql)
        {
            if (query.OrderBys.Count == 0) return sql;

            sql = $"{sql} {ORDER_BY}";
            foreach (var order in query.OrderBys)
            {
                sql = $"{sql} {order.Table}.{order.Column} {order.Order},";
            }

            return sql.TrimEnd(',');
        }

        public static string AppendLimit(IQuery query, string sql)
        {
            if (query.LimitParameter is null) return sql;

            return $"{sql} {LIMIT} {query.LimitParameter}";
        }

        public static string AppendOffset(IQuery query, string sql)
        {
            if (query.OffsetParameter is null) return sql;

            return $"{sql} {OFFSET} {query.OffsetParameter}";
        }
    }
}
