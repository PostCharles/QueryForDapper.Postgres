using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Extensions;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Test.Helpers;
using Test.TestModels;
using Xunit;

namespace Test.QueryTests
{
    [Trait("sut",nameof(Query))]
    public class QueryTest : BaseDispose
    {
        private Query _sut;

        public List<string> PassedColumnNames { get;}
        public List<string> PassedTableNames { get;}

        public Type Table { get; } = typeof(Right);
        public MemberInfo Column { get; } = typeof(Right).GetProperty(nameof(Right.RightId));
        public Type LeftTable { get; } = typeof(Left);
        public MemberInfo LeftColumn { get; } = typeof(Left).GetProperty(nameof(Left.LeftId));



        public QueryTest()
        {
            PassedColumnNames = new List<string>();
            PassedTableNames = new List<string>();
            

            Column = Table.GetProperty(nameof(Right.RightId));
            LeftTable = typeof(Left);
            LeftColumn = LeftTable.GetProperty(nameof(Left.LeftId));

            Query.ConfigureTo().NameColumnsWith(c => { PassedColumnNames.Add(c); return c; })
                               .NameTablesWith(t => { PassedTableNames.Add(t); return t; });

            _sut = new Query("");
        }


        [Fact]
        public void Constructor_StartTableStringPassedToTableNameMethod()
        {
            Query.FromTable<Table>();
            Assert.NotNull(PassedTableNames.Single(t => t == nameof(Table)));
        }



        [Fact]
        public void FromTable_ReturnsInstanceWithGenericSetToStartTable()
        {
            var sut = Query.FromTable<Table>();
            Assert.Equal(nameof(Table), sut.StartTable);
        }

        [Fact]
        public void SetLimitParameter_AssignsFormatedDapperParameter()
        {
            var parameter = "param";
            
            _sut.AddLimitParameter(parameter);

            Assert.Equal($"@{parameter}", _sut.LimitParameter);
        }

        [Fact]
        public void SetOffsetParameter_AssignsFormatedDapperParameter()
        {
            var parameter = "param";

            _sut.AddOffsetParameter(parameter);

            Assert.Equal($"@{parameter}", _sut.OffsetParameter);
        }

        [Fact]
        public void AddOrderBy_AddsOrderByToOrderBys()
        {
            var order = Order.ASC;

            _sut.AddOrderBy(Column, Table, order);

            var orderBy = _sut.OrderBys.Single();
            
            Assert.Equal(Column.Name, orderBy.Column);
            Assert.Equal(Table.Name, orderBy.Table);
            Assert.Equal(order.ToString(), orderBy.Order);
        }

        [Fact]
        public void AddOrderBy_PassesTableAndColumnToNameMethods()
        {
            _sut.AddOrderBy(Column, Table, Order.ASC);

            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
            Assert.Single(PassedColumnNames.Where(t => t == Column.Name));
        }

        [Fact]
        public void AddJoin_AddsJoinToJoins()
        {
            var joinType = JoinType.Inner;

            _sut.AddJoin(Column, Table, joinType);

            var join = _sut.Joins.Single();

            Assert.Equal(Column.Name, join.Column);
            Assert.Equal(Table.Name, join.Table);
            Assert.Equal(joinType.GetSql(), join.JoinType);
        }

        [Fact]
        public void AddJoin_PassesTableAndColumnToNameMethods()
        {
            _sut.AddJoin(Column, Table, JoinType.Inner);

            Assert.Single(PassedColumnNames.Where(c => c == Column.Name));
            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
        }

        [Fact]
        public void AddJoinWithStringColumn_AddsJoinToJoins()
        {
            var joinType = JoinType.Inner;

            _sut.AddJoin(Column.Name, Table, joinType);

            var join = _sut.Joins.Single();

            Assert.Equal(Column.Name, join.Column);
            Assert.Equal(Table.Name, join.Table);
            Assert.Equal(joinType.GetSql(), join.JoinType);
        }


        [Fact]
        public void AddJoinLeftAndRightWithStringColumns_PassesTablesToNameMethods()
        {
            _sut.AddJoin(Column.Name, Table, LeftColumn.Name, LeftTable, JoinType.Inner);

            Assert.Empty(PassedColumnNames.Where(c => c == Column.Name || c == LeftColumn.Name));
            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
            Assert.Single(PassedTableNames.Where(t => t == LeftTable.Name));
        }

        [Fact]
        public void AddJoinLeftAndRight_AddsJoinToJoins()
        {
            var joinType = JoinType.Inner;

            _sut.AddJoin(Column, Table, LeftColumn, LeftTable, joinType);

            var join = _sut.Joins.Single();

            Assert.Equal(Column.Name, join.Column);
            Assert.Equal(Table.Name, join.Table);
            Assert.Equal(LeftColumn.Name, join.LeftColumn);
            Assert.Equal(LeftTable.Name, join.LeftTable);
            Assert.Equal(joinType.GetSql(), join.JoinType);
        }

        [Fact]
        public void AddJoinLeftAndRight_PassesTablesAndColumnsToNameMethods()
        {
            _sut.AddJoin(Column, Table, LeftColumn, LeftTable, JoinType.Inner);

            Assert.Single(PassedColumnNames.Where(c => c == Column.Name));
            Assert.Single(PassedColumnNames.Where(c => c == LeftColumn.Name));
            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
            Assert.Single(PassedTableNames.Where(t => t == LeftTable.Name));
        }

        [Fact]
        public void AddJoinWithStringColumn_PassesTableToNameMethods()
        {
            _sut.AddJoin(Column.Name, Table, JoinType.Inner);

            Assert.Empty(PassedColumnNames.Where(c => c == Column.Name));
            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
        }

        [Fact]
        public void AddJoinLeftAndRightWithStringColumns_AddsJoinToJoins()
        {
            var joinType = JoinType.Inner;

            _sut.AddJoin(Column.Name, Table, LeftColumn.Name, LeftTable, joinType);

            var join = _sut.Joins.Single();

            Assert.Equal(Column.Name, join.Column);
            Assert.Equal(Table.Name, join.Table);
            Assert.Equal(LeftColumn.Name, join.LeftColumn);
            Assert.Equal(LeftTable.Name, join.LeftTable);
            Assert.Equal(joinType.GetSql(), join.JoinType);
        }

        [Fact]
        public void AddSelect_AddsSelectToSelects()
        {

            _sut.AddSelect(Column, Table);

            var select = _sut.Selects.Single();

            Assert.Equal(Column.Name, select.Column);
            Assert.Equal(Table.Name, select.Table);
        }

        [Fact]
        public void AddSelect_PassesTableAndColumnToNameMethods()
        {
            _sut.AddSelect(Column, Table);

            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
            Assert.Single(PassedColumnNames.Where(t => t == Column.Name));
        }

        [Fact]
        public void AddSelectWithStringColumn_AddsSelectToSelects()
        {
            var column = "column";

            _sut.AddSelect(column, Table);

            var select = _sut.Selects.Single();

            Assert.Equal(column, select.Column);
            Assert.Equal(Table.Name, select.Table);
        }

        [Fact]
        public void AddSelectWithStringColumn_PassesTableToNameMethods()
        {
            var column = "column";

            _sut.AddSelect(column, Table);

            Assert.Empty(PassedColumnNames.Where(t => t == column));
            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
        }

        [Fact]
        public void AddWhere_AddsWhereToWheres()
        {
            var op = Operator.And;
            var predicate = "predicate";

            _sut.AddWhere(Column, Table, predicate, op);

            var where = _sut.Wheres.Single();

            Assert.Equal(Column.Name, where.Column);
            Assert.Equal(Table.Name, where.Table);
            Assert.Equal(op.GetSql(), where.Operator);
            Assert.Equal(predicate, where.Predicate);
        }

        [Fact]
        public void AddWhere_PassesTableAndColumnToNameMethods()
        {
            _sut.AddWhere(Column, Table, "", Operator.And);

            Assert.Single(PassedTableNames.Where(t => t == Table.Name));
            Assert.Single(PassedColumnNames.Where(t => t == Column.Name));
        }
    }
}
