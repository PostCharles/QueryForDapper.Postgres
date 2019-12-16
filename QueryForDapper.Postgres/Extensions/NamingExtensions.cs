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
        public static string ToColumnName(this MemberInfo column)
        {
            var defintion = QueryConfiguration.Current.ColumnDefinitions.FirstOrDefault(d => d.TableType == column.ReflectedType && 
                                                                                             d.PropertyName == column.Name);
            if (defintion != null) return defintion.ColumnName;
            
            
            var attribute =  (QueryConfiguration.Current.ShouldUseColumnAttributes) ? column.GetCustomAttribute(typeof(ColumnAttribute)) : null;
            if (attribute != null) return ((ColumnAttribute)attribute).Name;

            return QueryConfiguration.Current.ColumnNameMethod(column.Name);
        }

        public static string ToColumnName(this string column)
        {
            return QueryConfiguration.Current.ColumnNameMethod(column);
        }

        public static string ToTableName(this Type table)
        {
            return QueryConfiguration.Current.TableNameMethod(table.Name);
        }
        public static string ToTableName(this string table)
        {
            return QueryConfiguration.Current.TableNameMethod(table);
        }
    }
}
