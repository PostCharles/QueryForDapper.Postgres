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
using Test.Helpers;
using Test.Mocks;
using Test.TestModels;
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
            var joinType = JoinType.Inner;

            Query.JoinOn<Table>(column, joinType);


            _mock.Verify(m => m.AddJoin(column, typeof(Table), joinType));
        }

        [Fact]
        public void JoinOnViaExpression_PassesValuesToAddJoin()
        {
            var table = typeof(Table);
            var member = ((Expression<Func<Table, object>>)((Table u) => u.TableId)).GetMemberInfo();
            var join = JoinType.LeftOuter;
            
            Query.JoinOn<Table>(u => u.TableId, join);

            _mock.Verify(m => m.AddJoin(member, table, join));
        }

        [Fact]
        public void JoinOnLeftRightViaString_PassesValuesToAddJoin()
        {
            var left = "Left";
            var right = "Right";
            Query.JoinOn<Left, Right>(left, right);

            _mock.Verify(m => m.AddJoin(right, typeof(Right), left, typeof(Left), JoinType.Inner));
        }

        [Fact]
        public void JoinOnLeftRightViaExpression_PassesValuesToAddJoin()
        {
            var left = typeof(Left).GetProperty(nameof(Left.LeftId));
            var right = typeof(Right).GetProperty(nameof(Right.RightId));

            Query.JoinOn<Left, Right>(l => l.LeftId, r => r.RightId);

            _mock.Verify(m => m.AddJoin(right, typeof(Right), left, typeof(Left), JoinType.Inner));
        }

        [Fact]
        public void CanJoin_LeftRightTypesAreEqual_ThrowsException()
        {
            var type = typeof(Left);
            var exception = Assert.Throws<JoinManyLeftRightEqualException>(() => Query.JoinMany<Left, Left>());
            Assert.Equal(String.Format(JoinManyLeftRightEqualException.ERRROR_MESSAGE, type.Name),
                         exception.Message);
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
            var left = joinMap.GetLeft(typeof(Left));
            _mock.Verify(m => m.AddJoin(left.Column, left.Table, JoinType.Inner));

            var right = joinMap.GetRight(typeof(Right));
            _mock.Verify(m => m.AddJoin(right.Column, right.Table, JoinType.Inner));
        }

        [Fact]
        public void JoinMany_JoinMapDefinedAndTablesAreReversed_AddsJoinsFromMapInReverse()
        {
            QueryConfiguration.Current.UseDefaultNaming().MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId);

            Query.JoinMany<Right, Left>();

            var joinMap = QueryConfiguration.Current.JoinMaps.Single();

            var left = joinMap.GetLeft(typeof(Right));
            _mock.Verify(m => m.AddJoin(left.Column, left.Table, JoinType.Inner));

            var right = joinMap.GetRight(typeof(Left));
            _mock.Verify(m => m.AddJoin(right.Column, right.Table, JoinType.Inner));
        }

        [Theory]
        [InlineData(JoinType.Inner)]
        [InlineData(JoinType.FullOuter)]
        public void JoinMany_JoinMapDefined_UsesPassedJoinTypeForJoinAndRightTables(JoinType joinType)
        {
            QueryConfiguration.Current.UseDefaultNaming().MapManyToMany<Left, Join, Right>(j => j.LeftId, j => j.RightId);

            Query.JoinMany<Left, Right>(joinType);

            var joinMap = QueryConfiguration.Current.JoinMaps.Single();
            var left = joinMap.GetLeft(typeof(Left));
            _mock.Verify(m => m.AddJoin(left.Column, left.Table, joinType));

            var right = joinMap.GetRight(typeof(Right));
            _mock.Verify(m => m.AddJoin(right.Column, right.Table, joinType));
        }
    }
}
