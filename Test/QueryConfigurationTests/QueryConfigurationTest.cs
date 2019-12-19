using Moq;
using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Interfaces;
using QueryForDapper.Postgres.Models;
using QueryForDapper.Postgres.NamingSchemes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Test.Helpers;
using Test.TestModels;
using Xunit;

namespace Test.QueryConfigurationTests
{
    [Trait("sut",nameof(QueryConfiguration))]
    public class QueryConfigurationTest : BaseDispose
    {

        [Fact]
        public void ColumnNameMethodSet_NotSet_AssignsDelegateToColumnNameMethod()
        {
            var field = typeof(QueryConfiguration).GetField("_columnNameMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.Null(field.GetValue(QueryConfiguration.Current));

            Func<string, string> nameDelegate = (string column) => column;

            QueryConfiguration.Current.ColumnNameMethod = nameDelegate;

            Assert.Equal(nameDelegate, QueryConfiguration.Current.ColumnNameMethod);
        }

        [Fact]
        public void TableNameMethodSet_NotSet_AssignsDelegateToTableNameMethod()
        {
            var field = typeof(QueryConfiguration).GetField("_tableNameMethod", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.Null(field.GetValue(QueryConfiguration.Current));

            Func<string, string> nameDelegate = (string table) => table;

            QueryConfiguration.Current.TableNameMethod = nameDelegate;

            Assert.Equal(nameDelegate, QueryConfiguration.Current.TableNameMethod);
        }

        [Fact]
        public void ColumnNameMethodGet_NotSet_ThrowsNameMethodNotSetException()
        {
            var exception = Assert.Throws<NameMethodNotSetException>(() => QueryConfiguration.Current.ColumnNameMethod);
            Assert.Contains("Column", exception.Message, StringComparison.OrdinalIgnoreCase);
        }
        [Fact]
        public void TableNameMethodGet_NotSet_ThrowsNameMethodNotSetException()
        {
            var exception = Assert.Throws<NameMethodNotSetException>(() => QueryConfiguration.Current.TableNameMethod);
            Assert.Contains("Table", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ColumnNameMethodSet_AlreadySet_ThrowsNameMethodSetException()
        {
            QueryConfiguration.Current.ColumnNameMethod = (c => c);

            var exception = Assert.Throws<NameMethodSetException>(() => QueryConfiguration.Current.ColumnNameMethod = (c => c));
            Assert.Contains("Column", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void TableNameMethodSet_TableNameMethodAlreadSet_ThrowsNameMethodSetException()
        {
            QueryConfiguration.Current.TableNameMethod = (t => t);

            var exception = Assert.Throws<NameMethodSetException>(() => Query.ConfigureTo().NameTablesWith(s => s));
            Assert.Contains("Table", exception.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void AssignNameMethodsFromScheme_AssignsRenameColumnToColumnNameMethod()
        {
            var scheme = new DefaultScheme();
            QueryConfiguration.Current.AssignNameMethodsFromScheme(scheme);

            Assert.Equal(scheme.RenameColumn, QueryConfiguration.Current.ColumnNameMethod);
        }

        [Fact]
        public void AssignNameMethodsFromScheme_AssignsRenameTableToTableNameMethod()
        {
            var scheme = new DefaultScheme();
            QueryConfiguration.Current.AssignNameMethodsFromScheme(scheme);

            Assert.Equal(scheme.RenameTable, QueryConfiguration.Current.TableNameMethod);
        }

        
    }
}
