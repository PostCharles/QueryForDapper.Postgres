using Moq;
using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Test.Fixtures;
using Test.Models;
using Xunit;

namespace Test.QueryTests
{
    [Trait("sut", nameof(JoinQueryExtensions))]
    public class JoinQueryExtensionsTest : BaseDispose
    {
        private IQuery Query { get => _mock.Object; }
        private Mock<IQuery> _mock;


        public JoinQueryExtensionsTest()
        {
            _mock = QueryMocks.Query;
        }
    

        public override void Dispose()
        {
            _mock.Reset();

            base.Dispose();
        }

        [Fact]
        public void JoinOnViaString_PassesValuesToAddJoin()
        {
            var column = "column";
            var joinType = JoinType.INNER;

            Query.JoinOn<UsingType>(column, joinType);


            _mock.Verify(m => m.AddJoin(column, typeof(UsingType), joinType));
        }

        [Fact]
        public void JoinOnViaExpression_PassesValuesToAddJoin()
        {
            var table = typeof(UsingType);
            var member = ((Expression<Func<UsingType, object>>)((UsingType u) => u.Declared)).GetMemberInfo();
            var join = JoinType.LEFT_OUTER;
            
            Query.JoinOn<UsingType>(u => u.Declared, join);

            _mock.Verify(m => m.AddJoin(member, table, join));
        }

        [Fact]
        public void JoinMany_NoMapExistForType_ThrowsJoinMapNotFoundException()
        {
            var exception = Assert.Throws<JoinMapNotFound>(() => Query.JoinMany<Left, Right>());

            Assert.Equal(String.Format(JoinMapNotFound.ERROR_MESSAGE, nameof(Left), nameof(Right)),
                         exception.Message);
        }

        [Fact]
        public void JoinMany_JoinMapDefined_AddsJoinsFromMap()
        {
            QueryConfiguration.Current.UseDefaultNaming().MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId);

            Query.JoinMany<Left, Right>();

            var joinMap = QueryConfiguration.Current.JoinMaps.Single();

            _mock.Verify(m => m.AddJoin(joinMap.LeftKey, joinMap.JoinTable, JoinType.INNER));
            _mock.Verify(m => m.AddJoin(joinMap.RightKey, joinMap.RightTable, JoinType.INNER));
        }
    }
}
