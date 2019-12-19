using Moq;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Test.Helpers;
using Test.Mocks;
using Test.TestModels;
using Xunit;

namespace Test.QueryTests
{
    [Trait("sut", nameof(SelectQueryExtensions))]
    public class SelectQueryExtensionsTest : BaseDispose
    {
        private IQuery Query { get => _mock.Object; }
        private Mock<IQuery> _mock;


        public SelectQueryExtensionsTest()
        {
            _mock = QueryMocks.Query;
        }


        public override void Dispose()
        {
            _mock.Reset();

            base.Dispose();
        }

        [Fact]
        public void Select_PassesTableAndStarToAddSelect()
        {
            Query.Select<Table>();

            _mock.Verify(m => m.AddSelect("*", typeof(Table)));
        }

        [Fact]
        public void SelectViaString_CallsAddSelect()
        {
            var column = "left_id";
            Query.Select<Table>(column);

            _mock.Verify(m => m.AddSelect(column, typeof(Table)));
        }

        [Fact]
        public void SelectViaStringArray_CallsAddSelectForEachColumn()
        {
            var idColumn = "left_id";
            var valueColumn = "value";

            Query.Select<Table>(idColumn, valueColumn);

            _mock.Verify(m => m.AddSelect(idColumn, typeof(Table)));
            _mock.Verify(m => m.AddSelect(valueColumn, typeof(Table)));
        }

        [Fact]
        public void SelectViaExpression_CallsAddSelect()
        {
            var member = typeof(Table).GetProperty(nameof(Table.TableId));
            
            Query.Select<Table>(l => l.TableId);
            
            _mock.Verify(m => m.AddSelect(member, typeof(Table)));
        }

        [Fact]
        public void SelectViaExpressionArray_CallsAddSelectForEachExpression()
        {
            var leftMember = typeof(Join).GetProperty(nameof(Join.LeftId));
            var rightMember = typeof(Join).GetProperty(nameof(Join.RightId));

            Query.Select<Join>(j => j.LeftId, j => j.RightId);

            _mock.Verify(m => m.AddSelect(leftMember, typeof(Join)));
            _mock.Verify(m => m.AddSelect(rightMember, typeof(Join)));
        }
    }
}
