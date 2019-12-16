using Moq;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Test.Fixtures;
using Xunit;

namespace Test.QueryTests
{
    [Trait("sut", nameof(MiscQueryExtensions))]
    public class MiscQueryExtensionsTest : BaseDispose
    {
        public IQuery Query { get => _mock.Object; }
        private Mock<IQuery> _mock;

        public MiscQueryExtensionsTest()
        {
            _mock = QueryMocks.Query;
        }

        public override void Dispose()
        {
            _mock.Reset();

            base.Dispose();
        }

        [Fact]
        public void SkipWith_PassesMemberNameToQuerySetOffsetParameter()
        {
            var offsetParameter = 10;
            Query.SkipWith(() => offsetParameter);

            _mock.Verify(m => m.AddOffsetParameter(nameof(offsetParameter)));
        }

        [Fact]
        public void TakeWith_PassesMemberNameToQuerySetLimitParameter()
        {
            var limitParameter = 10;
            Query.TakeWith(() => limitParameter);

            _mock.Verify(m => m.AddLimitParameter(nameof(limitParameter)));
        }
    }
}
