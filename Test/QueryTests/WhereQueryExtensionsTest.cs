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
        public void WhereComparedWith_CreatesPredicateWithParameter()
        {
            var parameter = "";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));

            Query.WhereComparedWith<Left>(l => l.LeftId, () => parameter, comparison: Comparison.NotEqual);

            var expectedPredicate = $"<> @{nameof(parameter)}";
            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

        [Fact]
        public void WhereCompared_CreatesPredicateWithValueAsLiteral()
        {
            var value = "CompareValue";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));

            Query.WhereCompared<Left>(l => l.LeftId, value, comparison: Comparison.GreaterThanEqual);

            var expectedPredicate = $">= '{value}'";
            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

        [Fact]
        public void WhereAnyWith_BuildsPredicateAndPassesValues()
        {
            var testParam = "";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));

            Query.WhereAnyWith<Left>(l => l.LeftId, () => testParam, Operator.NONE);
            
            var expectedPredicate = $"= ANY( @{nameof(testParam)} )";

            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

        [Fact]
        public void WhereLike_InjectsValueIntoPredicate()
        {
            var likeTerm = "TestTerm";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));

            Query.WhereLike<Left>(l => l.LeftId, likeTerm);
            
            var expectedPredicate = $"ILIKE '%' || '{likeTerm}' || '%'";

            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

        [Fact]
        public void WhereLikeWith_InjectsParameterIntoPredicate()
        {
            var parameter = "";
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));

            Query.WhereLikeWith<Left>(l => l.LeftId, () => parameter, like: Like.Begins, @case: Case.Sensitive);
            
            var expectedPredicate = $"LIKE @{nameof(parameter)} || '%'";

            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

        [Fact]
        public void WhereInSubQuery_ConvertsSubQueryToPredicate()
        {
            QueryForDapper.Postgres.Models.Query.ConfigureTo().UseDefaultNaming();

            var subQuery = QueryForDapper.Postgres.Models.
                           Query.FromTable<Right>().Select<Right>(r => r.RightId);
            var column = typeof(Left).GetProperty(nameof(Left.LeftId));

            Query.WhereInSubQuery<Left>(l => l.LeftId, subQuery);

            var expectedPredicate = $"IN ({subQuery.ToStatement()})";

            _mock.Verify(m => m.AddWhere(column, typeof(Left), expectedPredicate, Operator.NONE));
        }

    }
}
