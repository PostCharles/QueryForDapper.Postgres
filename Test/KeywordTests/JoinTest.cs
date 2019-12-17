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
        public void Constructor_ConvertsJoinTypeToSql()
        {
            var sut = new Join("", "", JoinType.FullOuter);

            Assert.Equal("FULL OUTER",
                         sut.JoinType);
        }

        [Fact]
        public void Constructor_SetsIsUsingToTrue()
        {
            var sut = new Join("", "", JoinType.FullOuter);

            Assert.True(sut.IsUsing);
        }

        [Fact]
        public void OverloadedConstructor_ConvertsJoinTypeToSql()
        {
            var sut = new Join("", "", "", "", JoinType.RightOuter);

            Assert.Equal("RIGHT OUTER",
                         sut.JoinType);
        }

        [Fact]
        public void Constructor_SetsIsUsingToFalse()
        {
            var sut = new Join("", "", "", "", JoinType.FullOuter);

            Assert.False(sut.IsUsing);
        }
    }
}
