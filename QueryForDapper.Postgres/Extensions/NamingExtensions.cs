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
        public static string ToColumnName(this string column)
        {
            return QueryConfiguration.Current.ColumnNameMethod(column);
        }

        public static string ToColumnName(this MemberInfo column, Type table)
        {
            var defintion = QueryConfiguration.Current.ColumnDefinitions.FirstOrDefault(d => d.IsMatch(column.Name, table));
            if (defintion != null) return defintion.ColumnName;

            var columnName =  GenerateColumnName(column, table);

            QueryConfiguration.Current.ColumnDefinitions.Add(new ColumnDefinition(table, column.Name, columnName));

            return columnName;
        }

        private static string GenerateColumnName(MemberInfo column, Type table)
        {
            if (QueryConfiguration.Current.ShouldUseColumnAttributes)
            {
                var columnFromTopMostClass = (column.ReflectedType == table) ? column : table.GetProperty(column.Name);
                var attribute = columnFromTopMostClass.GetCustomAttribute(typeof(ColumnAttribute)) as ColumnAttribute;

                if (attribute != null) return attribute.Name;
            }

            return QueryConfiguration.Current.ColumnNameMethod(column.Name);
        }

        public static string ToTableName(this string table)
        {
            return QueryConfiguration.Current.TableNameMethod(table);
        }

        public static string ToTableName(this Type table)
        {
            if (QueryConfiguration.Current.TableDefinitions.TryGetValue(table, out var tableName)) return tableName;

            var generatedTableName =  GenerateTableName(table);

            QueryConfiguration.Current.TableDefinitions.TryAdd(table, generatedTableName);

            return generatedTableName;

        }

        private static string GenerateTableName(Type table)
        {
            var attribute = (QueryConfiguration.Current.ShouldUseTableAttributes) ? table.GetCustomAttribute(typeof(TableAttribute)) : null;
            if (attribute != null) return ((TableAttribute)attribute).Name;

            return QueryConfiguration.Current.TableNameMethod(table.Name);
        }
    }
}
