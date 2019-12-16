using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Models.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.KeywordTests
{
    [Trait("sut", nameof(Where))]
    public class WhereTest : BaseDispose
    {
        [Fact]
        public void Constructor_OperatorIsNone_SetsOperatorToEmptyString()
        {
            var sut = new Where("", "", Operator.NONE, "");

            Assert.Empty(sut.Operator);
        }

        [Fact]
        public void Constructor_OperatorIsNotNone_SetsOperatorToStringOfValue()
        {
            var op = Operator.AND;
            var sut = new Where("", "", op, "");

            Assert.Equal(op.ToString(), sut.Operator);
        }
    }
}
