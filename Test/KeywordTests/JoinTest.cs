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
    [Trait("sut",nameof(Join))]
    public class JoinTest
    {
        [Fact]
        public void Constructor_ReplacesUnderScoreInJoinType()
        {
            var sut = new Join("", "", JoinType.FULL_OUTER);

            Assert.Equal(JoinType.FULL_OUTER.ToString().Replace('_', ' '),
                         sut.JoinType);
        }
    }
}
