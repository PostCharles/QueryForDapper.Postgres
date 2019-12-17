using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Interfaces;
using QueryForDapper.Postgres.Models;
using QueryForDapper.Postgres.NamingSchemes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Models;
using Xunit;

namespace Test.QueryConfigurationTests
{
    [Trait("sut",nameof(QueryConfigurationExtensions))]
    public class QueryConfigurationExtensionsTest : BaseDispose
    {

    
        [Fact]
        public void NameColumnsWith_SetsColumnNameMethod()
        {
            Func<string, string> columnDelegate = c => c;

            QueryConfiguration.Current.NameColumnsWith(columnDelegate);

            Assert.Equal(columnDelegate, QueryConfiguration.Current.ColumnNameMethod);
        }

        [Fact]
        public void NameTablesWith_SetsTableNameMethod()
        {
            Func<string, string> tableDelegate = c => c;

            QueryConfiguration.Current.NameTablesWith(tableDelegate);

            Assert.Equal(tableDelegate, QueryConfiguration.Current.TableNameMethod);
        }

        [Fact]
        public void UseDefaultNaming_AssignsNameMethodsFromDefaultScheme()
        {
            QueryConfiguration.Current.UseDefaultNaming();

            Assert.Equal(typeof(DefaultScheme).GetMethod(nameof(INamingScheme.RenameColumn)),
                         QueryConfiguration.Current.ColumnNameMethod.Method);
            Assert.Equal(typeof(DefaultScheme).GetMethod(nameof(INamingScheme.RenameTable)),
                         QueryConfiguration.Current.TableNameMethod.Method);
        }

        [Fact]
        public void UsePassthroughNaming_AssignsNameMethodsFromPassthroughScheme()
        {
            QueryConfiguration.Current.UsePassthroughNaming();

            Assert.Equal(typeof(PassthroughScheme).GetMethod(nameof(INamingScheme.RenameColumn)),
                         QueryConfiguration.Current.ColumnNameMethod.Method);
            Assert.Equal(typeof(PassthroughScheme).GetMethod(nameof(INamingScheme.RenameTable)),
                         QueryConfiguration.Current.TableNameMethod.Method);
        }

        [Fact]
        public void UseSnakeCaseNaming_AssignsNameMethodsFromSnakeCaseScheme()
        {
            QueryConfiguration.Current.UseSnakeCaseNaming();

            Assert.Equal(typeof(SnakeCaseScheme).GetMethod(nameof(INamingScheme.RenameColumn)),
                         QueryConfiguration.Current.ColumnNameMethod.Method);
            Assert.Equal(typeof(SnakeCaseScheme).GetMethod(nameof(INamingScheme.RenameTable)),
                         QueryConfiguration.Current.TableNameMethod.Method);
        }

        [Fact]
        public void UseCustomNamingScheme_AssignsNameMethodsFromPassedScheme()
        {
            QueryConfiguration.Current.UseCustomNamingScheme(new CustomNamingScheme());

            Assert.Equal(typeof(CustomNamingScheme).GetMethod(nameof(INamingScheme.RenameColumn)),
                         QueryConfiguration.Current.ColumnNameMethod.Method);
            Assert.Equal(typeof(CustomNamingScheme).GetMethod(nameof(INamingScheme.RenameTable)),
                         QueryConfiguration.Current.TableNameMethod.Method);
        }

        [Fact]
        public void UseColumnAttributeNames_SetsShouldUseColumnAttributesToTrue()
        {
            Assert.False(QueryConfiguration.Current.ShouldUseColumnAttributes);

            QueryConfiguration.Current.UseColumnAttributeNames();

            Assert.True(QueryConfiguration.Current.ShouldUseColumnAttributes);

        }

        [Fact]
        public void UseTableAttributeNames_SetsShouldUseTableAttributesToTrue()
        {
            Assert.False(QueryConfiguration.Current.ShouldUseTableAttributes);

            QueryConfiguration.Current.UseTableAttributeNames();

            Assert.True(QueryConfiguration.Current.ShouldUseTableAttributes);

        }

        [Fact]
        public void MapManyToManyViaExpression_NameMethodsNotSet_ThrowsConfigurationOrderException()
        {
            var exception = Assert.Throws<ConfigurationOrderException>(() => QueryConfiguration.Current.MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId));

            Assert.Equal(string.Format(ConfigurationOrderException.ERROR_MESSAGE, nameof(QueryConfigurationExtensions.MapManyToMany)),
                         exception.Message);
        }

        [Fact]
        public void MapManyToManyViaExpression_AddsMapToJoinMaps()
        {
            QueryConfiguration.Current.UsePassthroughNaming().MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId);

            var map = QueryConfiguration.Current.JoinMaps.Single();

            Assert.Equal(typeof(Left), map.LeftTable);
            Assert.Equal(typeof(Join), map.JoinTable);
            Assert.Equal(typeof(Right), map.RightTable);
            Assert.Equal(nameof(Join.LeftId), map.LeftKey);
            Assert.Equal(nameof(Join.RightId), map.RightKey);
        }

        [Fact]
        public void MapManyToManyViaExpression_PassesKeysToColumnNameMetod()
        {
            var renamedColumns = new List<string>();
            QueryConfiguration.Current.NameColumnsWith(s => { renamedColumns.Add(s); return s; })
                               .MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId);

            Assert.Contains(nameof(Join.LeftId), renamedColumns);
            Assert.Contains(nameof(Join.RightId), renamedColumns);
        }

        [Fact]
        public void MapManyToManyViaString_AddsMapToJoinMaps()
        {
            var leftKey = "leftKeyString";
            var rightKey = "rightKeyString";
            QueryConfiguration.Current.UsePassthroughNaming().MapManyToMany<Left, Join, Right>(leftKey, rightKey);

            var map = QueryConfiguration.Current.JoinMaps.Single();

            Assert.Equal(typeof(Left), map.LeftTable);
            Assert.Equal(typeof(Join), map.JoinTable);
            Assert.Equal(typeof(Right), map.RightTable);
            Assert.Equal(leftKey, leftKey);
            Assert.Equal(rightKey, rightKey);
        }

        [Fact]
        public void MapManyToManyViaString_DoesNotPassKeysToColumnNameMethod()
        {
            int callCount = 0;
            QueryConfiguration.Current.NameColumnsWith(s => { callCount++; return s; }).MapManyToMany<Left, Join, Right>("", "");
            Assert.Equal(0, callCount);
        }

        [Fact]
        public void DefineColumnName_AddsDefinitionToColumnDefintions()
        {
            var columnName = "Name";
            QueryConfiguration.Current.DefineColumnName<Table>(l => l.TableId, columnName);

            var defintion = QueryConfiguration.Current.ColumnDefinitions.Single();
            Assert.Equal(typeof(Table), defintion.TableType);
            Assert.Equal(nameof(Table.TableId), defintion.PropertyName);
            Assert.Equal(columnName, defintion.ColumnName);
        }

        [Fact]
        public void DefineColumnName_ColumnNameNotPassedToColumnNameMethod()
        {
            var callCount = 0;
            QueryConfiguration.Current.NameColumnsWith(s => { callCount++; return s; }).DefineColumnName<Table>(l => l.TableId, "");

            Assert.Equal(0, callCount);
        }


        [Fact]
        public void DefineColumnNameUsingColumnNaming_AddsDefintionToColumnDefintions()
        {
            var column = "MyColumn";
            QueryConfiguration.Current.UsePassthroughNaming().DefineColumnUsingColumnNaming<Table>(l => l.TableId, column);

            var defintion = QueryConfiguration.Current.ColumnDefinitions.Single();
            Assert.Equal(typeof(Table), defintion.TableType);
            Assert.Equal(nameof(Table.TableId), defintion.PropertyName);
            Assert.Equal(column, defintion.ColumnName);
        }

        [Fact]
        public void DefineColumnNameUsingColumnNaming_PassesColumnNameToColumnNameMethod()
        {
            var callCount = 0;
            QueryConfiguration.Current.NameColumnsWith(s => { callCount++; return s; }).DefineColumnUsingColumnNaming<Table>(l => l.TableId, "");

            Assert.Equal(1, callCount);
        }

        [Fact]
        public void DefineColumnNameUsingColumnNaming_CalledBeforeConfiguringNameMethods_ThrowsException()
        {
            var exception = Assert.Throws<ConfigurationOrderException>(() => QueryConfiguration.Current.DefineColumnUsingColumnNaming<Table>(l => l.TableId, ""));

            Assert.Equal(String.Format(ConfigurationOrderException.ERROR_MESSAGE,nameof(QueryConfigurationExtensions.DefineColumnUsingColumnNaming)),
                         exception.Message);
        }

        [Fact]
        public void DefineTableName_AddsDefintionToTableDefinitions()
        {
            var tableName = "Name";
            QueryConfiguration.Current.DefineTableName<Table>(tableName);

            var defintion = QueryConfiguration.Current.TableDefinitions.Single();

            Assert.Equal(typeof(Table), defintion.Key);
            Assert.Equal(tableName, defintion.Value);
        }

        [Fact]
        public void DefineTableNameUsingTableNaming_AddsDefintionToTableDefintions()
        {
            var column = "MyTable";
            QueryConfiguration.Current.UsePassthroughNaming().DefineTableUsingTableNaming<Table>(column);

            var defintion = QueryConfiguration.Current.TableDefinitions.Single();
            Assert.Equal(typeof(Table), defintion.Key);
            Assert.Equal(column, defintion.Value);
        }

        [Fact]
        public void DefineTableNameUsingTableNaming_PassesTableNameToTableNameMethod()
        {
            var callCount = 0;
            QueryConfiguration.Current.NameTablesWith(s => { callCount++; return s; }).DefineTableUsingTableNaming<Table>("");

            Assert.Equal(1, callCount);
        }

        [Fact]
        public void DefineTableNameUsingTableNaming_CalledBeforeConfiguringNameMethods_ThrowsException()
        {
            var exception = Assert.Throws<ConfigurationOrderException>(() => QueryConfiguration.Current.DefineTableUsingTableNaming<Table>(""));

            Assert.Equal(String.Format(ConfigurationOrderException.ERROR_MESSAGE, nameof(QueryConfigurationExtensions.DefineTableUsingTableNaming)),
                         exception.Message);
        }
    }

    public class CustomNamingScheme : INamingScheme
    {
        public string RenameColumn(string column)
        {
            throw new NotImplementedException();
        }

        public string RenameTable(string table)
        {
            throw new NotImplementedException();
        }
    }
}
