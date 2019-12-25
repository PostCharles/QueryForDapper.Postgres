using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Extensions
{
    public static class NamingExtensions
    {
        public static string ToColumnName(this MemberInfo column, Type table)
        {
            var defintion = QueryConfiguration.Current.ColumnDefinitions.FirstOrDefault(d => d.IsMatch(column.Name, table));
            if (defintion != null) return defintion.ColumnName;

            var attribute = (QueryConfiguration.Current.ShouldUseColumnAttributes) ? GetColumnAttribute(column, table) : null;
            if (attribute != null) return ((ColumnAttribute)attribute).Name;

            return QueryConfiguration.Current.ColumnNameMethod(column.Name);
        }

        private static Attribute GetColumnAttribute(MemberInfo column, Type table)
        {
            var columnFromTopMostClass = (column.ReflectedType == table) ? column : table.GetProperty(column.Name);

            return columnFromTopMostClass.GetCustomAttribute(typeof(ColumnAttribute));
        }



        public static string ToColumnName(this string column)
        {
            return QueryConfiguration.Current.ColumnNameMethod(column);
        }

        public static string ToTableName(this Type table)
        {
            if (QueryConfiguration.Current.TableDefinitions.TryGetValue(table, out var tableName)) return tableName;

            var attribute = (QueryConfiguration.Current.ShouldUseTableAttributes) ? table.GetCustomAttribute(typeof(TableAttribute)) : null;
            if (attribute != null) return ((TableAttribute)attribute).Name;

            return QueryConfiguration.Current.TableNameMethod(table.Name);

        }
        public static string ToTableName(this string table)
        {
            return QueryConfiguration.Current.TableNameMethod(table);
        }
    }
}
