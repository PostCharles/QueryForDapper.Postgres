using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Test.Models;
using Xunit;

namespace Test.QueryTests
{
    [Trait("sut",nameof(Query))]
    public class QueryTest : BaseDispose
    {
        private Query _sut;

        public List<string> PassedColumnNames { get;}
        public List<string> PassedTableNames { get;}
        
        public (MemberInfo Column, Type Table) TestTable { get; }

        public QueryTest()
        {
            PassedColumnNames = new List<string>();
            PassedTableNames = new List<string>();
            TestTable = (typeof(Left).GetProperty(nameof(Left.LeftId)),
                         typeof(Left));

            Query.ConfigureTo().NameColumnsWith(c => { PassedColumnNames.Add(c); return c; })
                               .NameTablesWith(t => { PassedTableNames.Add(t); return t; });

            _sut = new Query("");
        }


        [Fact]
        public void Constructor_StartTableStringPassedToTableNameMethod()
        {
            Query.FromTable<Left>();
            Assert.NotNull(PassedTableNames.Single(t => t == nameof(Left)));
        }



        [Fact]
        public void FromTable_ReturnsInstanceWithGenericSetToStartTable()
        {
            var sut = Query.FromTable<Left>();
            Assert.Equal(nameof(Left), sut.StartTable);
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

            _sut.AddOrderBy(TestTable.Column, TestTable.Table, order);

            var orderBy = _sut.OrderBys.Single();
            
            Assert.Equal(TestTable.Column.Name, orderBy.Column);
            Assert.Equal(TestTable.Table.Name, orderBy.Table);
            Assert.Equal(order.ToString(), orderBy.Order);
        }

        [Fact]
        public void AddOrderBy_PassesTableAndColumnToNameMethods()
        {
            _sut.AddOrderBy(TestTable.Column, TestTable.Table, Order.ASC);

            Assert.Single(PassedTableNames.Where(t => t == TestTable.Table.Name));
            Assert.Single(PassedColumnNames.Where(t => t == TestTable.Column.Name));
        }

        [Fact]
        public void AddJoin_AddsJoinToJoins()
        {
            var joinType = JoinType.INNER;

            _sut.AddJoin(TestTable.Column, TestTable.Table, joinType);

            var join = _sut.Joins.Single();

            Assert.Equal(TestTable.Column.Name, join.Column);
            Assert.Equal(TestTable.Table.Name, join.Table);
            Assert.Equal(joinType.ToString(), join.JoinType);
        }

        [Fact]
        public void AddJoin_PassesTableAndColumnToNameMethods()
        {
            _sut.AddJoin(TestTable.Column, TestTable.Table, JoinType.INNER);

            Assert.Single(PassedTableNames.Where(t => t == TestTable.Table.Name));
            Assert.Single(PassedColumnNames.Where(t => t == TestTable.Column.Name));
        }

        [Fact]
        public void AddJoinWithStringColumn_AddsJoinToJoins()
        {
            var joinType = JoinType.INNER;
            var column = "column";

            _sut.AddJoin(column, TestTable.Table, joinType);

            var join = _sut.Joins.Single();

            Assert.Equal(column, join.Column);
            Assert.Equal(TestTable.Table.Name, join.Table);
            Assert.Equal(joinType.ToString(), join.JoinType);
        }

        [Fact]
        public void AddJoinWithStringColumn_PassesTableToNameMethods()
        {
            var column = "column";
            _sut.AddJoin(column, TestTable.Table, JoinType.INNER);

            Assert.Empty(PassedColumnNames.Where(t => t == column));
            Assert.Single(PassedTableNames.Where(t => t == TestTable.Table.Name));
        }

        [Fact]
        public void AddSelect_AddsSelectToSelects()
        {

            _sut.AddSelect(TestTable.Column, TestTable.Table);

            var select = _sut.Selects.Single();

            Assert.Equal(TestTable.Column.Name, select.Column);
            Assert.Equal(TestTable.Table.Name, select.Table);
        }

        [Fact]
        public void AddSelect_PassesTableAndColumnToNameMethods()
        {
            _sut.AddSelect(TestTable.Column, TestTable.Table);

            Assert.Single(PassedTableNames.Where(t => t == TestTable.Table.Name));
            Assert.Single(PassedColumnNames.Where(t => t == TestTable.Column.Name));
        }

        [Fact]
        public void AddSelectWithStringColumn_AddsSelectToSelects()
        {
            var column = "column";

            _sut.AddSelect(column, TestTable.Table);

            var select = _sut.Selects.Single();

            Assert.Equal(column, select.Column);
            Assert.Equal(TestTable.Table.Name, select.Table);
        }

        [Fact]
        public void AddSelectWithStringColumn_PassesTableToNameMethods()
        {
            var column = "column";

            _sut.AddSelect(column, TestTable.Table);

            Assert.Empty(PassedColumnNames.Where(t => t == column));
            Assert.Single(PassedTableNames.Where(t => t == TestTable.Table.Name));
        }

        [Fact]
        public void AddWhere_AddsWhereToWheres()
        {
            var op = Operator.AND;
            var predicate = "predicate";

            _sut.AddWhere(TestTable.Column, TestTable.Table, predicate, op);

            var where = _sut.Wheres.Single();

            Assert.Equal(TestTable.Column.Name, where.Column);
            Assert.Equal(TestTable.Table.Name, where.Table);
            Assert.Equal(op.ToString(), where.Operator);
            Assert.Equal(predicate, where.Predicate);
        }

        [Fact]
        public void AddWhere_PassesTableAndColumnToNameMethods()
        {
            _sut.AddWhere(TestTable.Column, TestTable.Table, "", Operator.AND);

            Assert.Single(PassedTableNames.Where(t => t == TestTable.Table.Name));
            Assert.Single(PassedColumnNames.Where(t => t == TestTable.Column.Name));
        }
    }
}
