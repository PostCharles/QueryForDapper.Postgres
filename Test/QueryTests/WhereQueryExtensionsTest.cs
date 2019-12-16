using Moq;
using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Extensions;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Fixtures;
using Test.Models;
using Xunit;

namespace Test.QueryTests
{
    [Trait("sut", nameof(WhereQueryExtensions))]
    public class WhereQueryExtensionsTest : BaseDispose
    {
        private IQuery Query { get => _mock.Object; }
        private Mock<IQuery> _mock;


        public WhereQueryExtensionsTest()
        {
            _mock = QueryMocks.Query;
        }


        public override void Dispose()
        {
            _mock.Reset();

            base.Dispose();
        }

        [Fact]
        public void WhereAnyWith_BuildsPredicateAndPassesValues()
        {
            var testParam = "";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));
            var predicate = $"= ANY( @{nameof(testParam)} )";

            Query.WhereAnyWith<Left>(l => l.LeftId, () => testParam, Operator.NONE);

            _mock.Verify(m => m.AddWhere(column, typeof(Left), predicate, Operator.NONE));
        }

        [Fact]
        public void WhereLike_InjectsValueIntoPredicate()
        {
            var likeTerm = "TestTerm";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));
            var expectedPredicate = $"ILIKE '%' || '{likeTerm}' || '%'";

            Query.WhereLike<Left>(l => l.LeftId, likeTerm);

            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

        [Fact]
        public void WhereLikeWith_InjectsParameterIntoPredicate()
        {
            var parameter = "";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));
            var expectedPredicate = $"LIKE @{nameof(parameter)} || '%'";

            Query.WhereLikeWith<Left>(l => l.LeftId, () => parameter, like: Like.Begins, @case: Case.Sensitive);

            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

        [Fact]
        public void WhereInSubQuery_ConvertsSubQueryToPredicate()
        {
            QueryForDapper.Postgres.Models.Query.ConfigureTo().UseDefaultNaming();

            var subQuery = QueryForDapper.Postgres.Models.
                           Query.FromTable<Right>().Select<Right>(r => r.RightId);

            Query.WhereInSubQuery<Left>(l => l.LeftId, subQuery);

            var column = typeof(Left).GetProperty(nameof(Left.LeftId));
            var predicate = $"IN ({subQuery.ToStatement()})";
            _mock.Verify(m => m.AddWhere(column, typeof(Left), predicate, Operator.NONE));
        }

    }
}
