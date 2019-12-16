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

            Query.ConfigureTo().NameColumnsWith(columnDelegate);

            Assert.Equal(columnDelegate, QueryConfiguration.Current.ColumnNameMethod);
        }

        [Fact]
        public void NameTablesWith_SetsTableNameMethod()
        {
            Func<string, string> tableDelegate = c => c;

            Query.ConfigureTo().NameTablesWith(tableDelegate);

            Assert.Equal(tableDelegate, QueryConfiguration.Current.TableNameMethod);
        }

        [Fact]
        public void UseDefaultNaming_AssignsNameMethodsFromDefaultScheme()
        {
            Query.ConfigureTo().UseDefaultNaming();

            Assert.Equal(typeof(DefaultScheme).GetMethod(nameof(INamingScheme.RenameColumn)),
                         QueryConfiguration.Current.ColumnNameMethod.Method);
            Assert.Equal(typeof(DefaultScheme).GetMethod(nameof(INamingScheme.RenameTable)),
                         QueryConfiguration.Current.TableNameMethod.Method);
        }

        [Fact]
        public void UsePassthroughNaming_AssignsNameMethodsFromPassthroughScheme()
        {
            Query.ConfigureTo().UsePassthroughNaming();

            Assert.Equal(typeof(PassthroughScheme).GetMethod(nameof(INamingScheme.RenameColumn)),
                         QueryConfiguration.Current.ColumnNameMethod.Method);
            Assert.Equal(typeof(PassthroughScheme).GetMethod(nameof(INamingScheme.RenameTable)),
                         QueryConfiguration.Current.TableNameMethod.Method);
        }

        [Fact]
        public void UseSnakeCaseNaming_AssignsNameMethodsFromSnakeCaseScheme()
        {
            Query.ConfigureTo().UseSnakeCaseNaming();

            Assert.Equal(typeof(SnakeCaseScheme).GetMethod(nameof(INamingScheme.RenameColumn)),
                         QueryConfiguration.Current.ColumnNameMethod.Method);
            Assert.Equal(typeof(SnakeCaseScheme).GetMethod(nameof(INamingScheme.RenameTable)),
                         QueryConfiguration.Current.TableNameMethod.Method);
        }

        [Fact]
        public void UseColumnAttributeNames_SetsShouldUseColumnAttributesToTrue()
        {
            Assert.False(QueryConfiguration.Current.ShouldUseColumnAttributes);

            Query.ConfigureTo().UseColumnAttributeNames();

            Assert.True(QueryConfiguration.Current.ShouldUseColumnAttributes);

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
            Query.ConfigureTo().UsePassthroughNaming().MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId);

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
            Query.ConfigureTo().NameColumnsWith(s => { renamedColumns.Add(s); return s; })
                               .MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId);

            Assert.Contains(nameof(Join.LeftId), renamedColumns);
            Assert.Contains(nameof(Join.RightId), renamedColumns);
        }

        [Fact]
        public void MapManyToManyViaString_AddsMapToJoinMaps()
        {
            var leftKey = "leftKeyString";
            var rightKey = "rightKeyString";
            Query.ConfigureTo().UsePassthroughNaming().MapManyToMany<Left, Join, Right>(leftKey, rightKey);

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
            Query.ConfigureTo().NameColumnsWith(s => { callCount++; return s; }).MapManyToMany<Left, Join, Right>("", "");
            Assert.Equal(0, callCount);
        }

        [Fact]
        public void DefineColumnName_AddsDefinitionToColumnDefintions()
        {
            var columnName = "Name";
            Query.ConfigureTo().DefineColumnName<Left>(l => l.LeftId, columnName);

            var defintion = QueryConfiguration.Current.ColumnDefinitions.Single();
            Assert.Equal(typeof(Left), defintion.TableType);
            Assert.Equal(nameof(Left.LeftId), defintion.PropertyName);
            Assert.Equal(columnName, defintion.ColumnName);
        }

        [Fact]
        public void DefineColumnName_ColumnNameNotPassedToColumnNameMethod()
        {
            var callCount = 0;
            Query.ConfigureTo().NameColumnsWith(s => { callCount++; return s; }).DefineColumnName<Left>(l => l.LeftId, "");

            Assert.Equal(0, callCount);
        }

        [Fact]
        public void DefineColumnNameUsingColumnNaming_AddsDefintionToColumnDefintions()
        {
            var column = "MyColumn";
            Query.ConfigureTo().UsePassthroughNaming().DefineColumnUsingColumnNaming<Left>(l => l.LeftId, column);

            var defintion = QueryConfiguration.Current.ColumnDefinitions.Single();
            Assert.Equal(typeof(Left), defintion.TableType);
            Assert.Equal(nameof(Left.LeftId), defintion.PropertyName);
            Assert.Equal(column, defintion.ColumnName);
        }

        [Fact]
        public void DefineColumnNameUsingColumnNaming_PassesColumnNameToColumnNameMethod()
        {
            var callCount = 0;
            Query.ConfigureTo().NameColumnsWith(s => { callCount++; return s; }).DefineColumnUsingColumnNaming<Left>(l => l.LeftId, "");

            Assert.Equal(1, callCount);
        }

        [Fact]
        public void DefineColumnNameUsingColumnNaming_CalledBeforeConfiguringNameMethods_ThrowsException()
        {
            var exception = Assert.Throws<ConfigurationOrderException>(() => Query.ConfigureTo().DefineColumnUsingColumnNaming<Left>(l => l.LeftId, ""));

            Assert.Equal(String.Format(ConfigurationOrderException.ERROR_MESSAGE,nameof(QueryConfigurationExtensions.DefineColumnUsingColumnNaming)),
                         exception.Message);
        }
    }
}
