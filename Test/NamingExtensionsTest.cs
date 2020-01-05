using QueryForDapper.Postgres.Models;
using QueryForDapper.Postgres.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Test.TestModels;
using Test.Helpers;

namespace Test
{
    [Trait("sut", nameof(NamingExtensions))]
    public class NamingExtensionsTest : BaseDispose
    {
        public const string SUFFIX = "-result";
        public List<string> PassedColumnNames { get; }
        public List<string> PassedTableNames { get; }

        public NamingExtensionsTest()
        {
            PassedColumnNames = new List<string>();
            PassedTableNames = new List<string>();

            QueryConfiguration.Current.NameColumnsWith(c => { PassedColumnNames.Add(c); return c + SUFFIX; })
                                      .NameTablesWith(t => { PassedTableNames.Add(t); return t + SUFFIX; }); 
        }

        [Fact]
        public void ToColumnNameByString_PassesValueToColumnNameMethod()
        {
            var column = "column";

            var result = column.ToColumnName();

            Assert.Single(PassedColumnNames.Where(c => c == column));
            Assert.Equal($"{column}{SUFFIX}", result);
        }

        [Fact]
        public void ToColumnNameByMemberInfo_PassesValueToColumnNameMethod()
        {
            var tableType = typeof(Table);
            var member = tableType.GetProperty(nameof(Table.TableId));

            var result = member.ToColumnName(tableType);

            Assert.Single(PassedColumnNames.Where(c => c == member.Name));
            Assert.Equal($"{member.Name}{SUFFIX}", result);
        }

        [Fact]
        public void ToColumnNameByMemberInfo_HasColumnDefinition_ReturnsValueFromDefinition()
        {
            var definedColumn = "ColumnName";
            QueryConfiguration.Current.DefineColumnName<UsingType>(l => l.Declared, definedColumn);

            var tableType = typeof(UsingType);
            var member = tableType.GetProperty(nameof(UsingType.Declared));

            var result = member.ToColumnName(tableType);

            Assert.Empty(PassedColumnNames);
            Assert.Equal(definedColumn, result);
        }

        [Fact]
        public void ToColumnNameByMemberInfo_HasColumnAttribute_ReturnsNameFromAttribute()
        {
            var tableType = typeof(AttributedType);
            var member = tableType.GetProperty(nameof(AttributedType.id));

            QueryConfiguration.Current.ShouldUseColumnAttributes = true;

            var result = member.ToColumnName(tableType);

            Assert.Equal(AttributedType.COLUMN_ATTRIBUTE_VALUE, result);
        }

        [Fact]
        public void ToColumnNameByMemberInfo_HasColumnAttributeAndColumnDefinition_ReturnsNameFromDefinition()
        {
            var definedColumn = "defined_column_name";
            QueryConfiguration.Current.DefineColumnName<AttributedType>(c => c.id, definedColumn);

            var tableType = typeof(AttributedType);
            var member = tableType.GetProperty(nameof(AttributedType.id));

            var result = member.ToColumnName(tableType);

            Assert.Equal(definedColumn, result);
        }

        [Fact]
        public void ToColumnNameByMemberInfo_ColumnFromGenericBaseType_ReturnsColumnNameFromAttributeOnDerivedType()
        {
            QueryConfiguration.Current.UseColumnAttributeNames();
           
            var tableType = typeof(UsingAttributedType);
            var member = typeof(DeclaringType).GetProperty(nameof(UsingAttributedType.Declared));

            var result = member.ToColumnName(tableType);

            Assert.Equal(UsingAttributedType.COLUMN_ATTRIBUTE_NAME, result);
        }

        [Fact]
        public void ToColumnNameByMemberInfo_ColumnFromGenericBaseType_ReturnsColumnNameDefinedOnDerivedType()
        {
            var columnName = "TestColumnName";
            QueryConfiguration.Current.DefineColumnName<UsingType>(t => t.Declared, columnName);

            var tableType = typeof(UsingType);
            var member = typeof(DeclaringType).GetProperty(nameof(UsingType.Declared));

            var result = member.ToColumnName(tableType);

            Assert.Equal(columnName, result);
        }

        [Fact]
        public void ToTableNameByTable_PassesValueToTableNameMethod()
        {
            var table = typeof(Left);

            var result = table.ToTableName();

            Assert.Single(PassedTableNames.Where(c => c == table.Name));
            Assert.Equal($"{table.Name}{SUFFIX}", result);
        }

        [Fact]
        public void ToTableNameByType_HasTableDefinition_ReturnsValueFromDefinition()
        {
            var definedTable = "TableName";
            QueryConfiguration.Current.DefineTableName<UsingType>(definedTable);

            var result = typeof(UsingType).ToTableName();
            
            Assert.Empty(PassedTableNames);
            Assert.Equal(definedTable, result);
        }

        [Fact]
        public void ToTableNameByType_HasTableAttribute_ReturnsNameFromAttribute()
        {
            QueryConfiguration.Current.ShouldUseTableAttributes = true;

            var result = typeof(AttributedType).ToTableName();

            Assert.Equal(AttributedType.TABLE_ATTRIBUTE_VALUE, result);
        }

        [Fact]
        public void ToTableNameByType_HasTableAttributeAndTableDefinition_ReturnsNameFromDefinition()
        {
            var definedTable = "defined_table_name";
            QueryConfiguration.Current.DefineTableName<AttributedType>(definedTable);


            var result = typeof(AttributedType).ToTableName();

            Assert.Equal(definedTable, result);
        }

        [Fact]
        public void ToTableNameByString_PassesValueToTableNameMethod()
        {
            var tableName = typeof(Left).Name;

            var result = tableName.ToTableName();

            Assert.Single(PassedTableNames.Where(c => c == tableName));
            Assert.Equal($"{tableName}{SUFFIX}", result);
        }
    }
}

