using Moq;
using QueryForDapper.Postgres.Enums;
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
    [Trait("sut", nameof(OrderByQueryExtensions))]
    public class OrderByQueryExtensionsTest : BaseDispose
    {
        public IQuery Query { get => _mock.Object; }

        private Mock<IQuery> _mock;

        public OrderByQueryExtensionsTest()
        {
            _mock = QueryMocks.Query;
        }

        public override void Dispose()
        {
            _mock.Reset();
            base.Dispose();
        }


        [Fact]
        public void OrderBy_PassesValueToAddOrderBy()
        {
            var member = typeof(Left).GetProperty(nameof(Left.LeftId));

            Query.OrderBy<Left>(l => l.LeftId);

            _mock.Verify(m => m.AddOrderBy(member, typeof(Left), Order.ASC));
        }
    }
}
