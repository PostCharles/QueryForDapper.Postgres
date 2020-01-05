using Moq;
using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Helpers;
using Test.Mocks;
using Test.TestModels;
using Xunit;
using static QueryForDapper.Postgres.Models.ToStatementQueryExtensions;

namespace Test.QueryTests
{
    [Trait("sut",nameof(ToStatementQueryExtensions))]
    public class ToStatementQueryExtensionsTest :BaseDispose
    {
        private const string BASE_STATEMENT = "Sql Statement";
        
        public Query _query { get; }

        public ToStatementQueryExtensionsTest()
        {
            QueryConfiguration.Current.UsePassthroughNaming();
            _query = new Query("");
        }

        [Fact]
        public void ToStatement_Executes_SubStatementInOrder()
        {

        }

        [Fact]
        public void ToStatement_CreatesStatementInOrder()
        {
            var param = "";
            var sql = Query.FromTable<Left>()
                           .JoinOn<Join>(j => j.JoinId)
                           .WhereLike<Right>(r => r.RightId, "Test")
                           .OrderBy<Left>(l => l.LeftId)
                           .SkipWith(() => param)
                           .TakeWith(() => param)
                           .ToStatement();

            Assert.Equal($"SELECT * FROM {nameof(Left)} " +
                        $"INNER JOIN {nameof(Join)} USING ({nameof(Join.JoinId)}) " +
                        $"WHERE {nameof(Right)}.{nameof(Right.RightId)} ILIKE '%' || 'Test' || '%' " +
                        $"ORDER BY {nameof(Left)}.{nameof(Left.LeftId)} ASC " +
                        $"LIMIT @param OFFSET @param",
                        sql);
        }

        [Fact]
        public void BuildSelect_QueryHasNoSelects_SelectsAll()
        {
            var query = Query.FromTable<Left>();
            Assert.Equal($"SELECT * FROM {nameof(Left)}",
                         BuildSelect(query));
        }
        
        [Fact]
        public void BuildSelect_QueryHasMultipleSelects_ConcatsSelects()
        {
            var @as = "TABLE_ID";
            var query = Query.FromTable<Left>()
                             .Select<Left>(l => l.LeftId)
                             .Select<Right>(r => r.RightId)
                             .SelectAs<Table>(t => t.TableId, @as)
                             .Select<Join>(j => j.JoinId);

            Assert.Equal($"SELECT {nameof(Left)}.{nameof(Left.LeftId)}, {nameof(Right)}.{nameof(Right.RightId)}, {nameof(Table)}.{nameof(Table.TableId)} AS {@as}, {nameof(Join)}.{nameof(Join.JoinId)} FROM {nameof(Left)}",
                         BuildSelect(query));
        }

        [Fact]
        public void AppendJoins_QueryHasNoJoins_ReturnsUnalteredStatement()
        {
            Assert.Equal(BASE_STATEMENT, AppendJoins(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendJoin_JoinIsUsing_AppendsJoinWithUsingStatement()
        {
            _query.JoinOn<Table>(t => t.TableId);

            Assert.True(_query.Joins.Single().IsUsing);

            Assert.Equal($"{BASE_STATEMENT} INNER JOIN {nameof(Table)} USING ({nameof(Table.TableId)})",
                         AppendJoins(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendJoin_JoinNotUsing_AppendsJoinWithOnStatement()
        {
            _query.JoinOn<Join, Right>(j => j.RightId, r => r.RightId);

            Assert.False(_query.Joins.Single().IsUsing);

            Assert.Equal($"{BASE_STATEMENT} INNER JOIN {nameof(Right)} ON {nameof(Join)}.{nameof(Join.RightId)} = {nameof(Right)}.{nameof(Right.RightId)}",
                         AppendJoins(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendJoins_QueryHasJoins_ReturnsStatementWithAppenedJoins()
        {
            
            _query.JoinOn<Join>(j => j.LeftId)
                 .JoinOn<Right>(r => r.RightId, JoinType.RightOuter);

            Assert.Equal($"{BASE_STATEMENT} INNER JOIN {nameof(Join)} USING ({nameof(Join.LeftId)}) RIGHT OUTER JOIN {nameof(Right)} USING ({nameof(Right.RightId)})",
                         AppendJoins(_query, BASE_STATEMENT));
        }



        [Fact]
        public void AppendWheres_QueryHasNoWheres_ReturnsUnaliteredStatement()
        {
            Assert.Equal(BASE_STATEMENT, AppendWheres(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendWheres_QueryHasWheres_ReturnsStatementWtihAppendedWheres()
        {
            var parameter = "";
            var term = "SearchTerm";
            var subQuery = Query.FromTable<Right>().Select<Right>(r => r.RightId);

            _query.WhereAnyWith<Left>(l => l.LeftId, () => parameter)
                 .WhereLike<Join>(j => j.JoinId, term, Operator.And)
                 .WhereInSubQuery<Right>(r => r.RightId, subQuery, Operator.Not);

            Assert.Equal($"{BASE_STATEMENT} WHERE {nameof(Left)}.{nameof(Left.LeftId)} = ANY( @{nameof(parameter)} ) " +
                         $"AND {nameof(Join)}.{nameof(Join.JoinId)} ILIKE '%' || '{term}' || '%' " +
                         $"NOT {nameof(Right)}.{nameof(Right.RightId)} IN ({subQuery.ToStatement()})",
                         AppendWheres(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendOrderBys_QueryHasNoOrderBys_ReturnsUnalteredStatement()
        {
            Assert.Equal(BASE_STATEMENT, AppendOrderBys(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendOrderBys_QueryHasOrderBys_ReturnsStatementWithAppenededOrderBys()
        {
            _query.OrderBy<Left>(l => l.LeftId)
                 .OrderBy<Right>(r => r.RightId, Order.DESC);

            Assert.Equal($"{BASE_STATEMENT} ORDER BY {nameof(Left)}.{nameof(Left.LeftId)} ASC, {nameof(Right)}.{nameof(Right.RightId)} DESC",
                         AppendOrderBys(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendLimit_QueryNoLimitParameter_ReturnsUnalteredStatement()
        {
            Assert.Equal(BASE_STATEMENT, AppendLimit(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendLimit_QueryHasLimitParameter_ReturnsAppendedLimit()
        {
            var parameter = "";
            _query.TakeWith(() => parameter);

            Assert.Equal($"{BASE_STATEMENT} LIMIT @{nameof(parameter)}",
                         AppendLimit(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendOffset_QueryNoOffsetParameter_ReturnsUnalteredStatement()
        {
            Assert.Equal(BASE_STATEMENT, AppendOffset(_query, BASE_STATEMENT));
        }

        [Fact]
        public void AppendOffset_QueryHasOffsetParameter_ReturnsAppendedOffset()
        {
            var parameter = "";
            _query.SkipWith(() => parameter);

            Assert.Equal($"{BASE_STATEMENT} OFFSET @{nameof(parameter)}",
                         AppendOffset(_query, BASE_STATEMENT));
        }
    }
}
