using QueryForDapper.Postgres.Attributes;
using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    [Trait("sut", nameof(EnumExtensions))]
    public class EnumExtensionsTest
    {
        [Theory]
        [InlineData(Operator.None, "")]
        [InlineData(Operator.Not, "NOT")]
        [InlineData(Case.Sensitive, "LIKE")]
        [InlineData(JoinType.FullOuter, "FULL OUTER")]
        [InlineData(Like.Anywhere, "'%' || {0} || '%'")]
        public void GetSql_EnumValueHasSqlPartialAttribute_ReturnsSqlValue(Enum @enum, string expectedValue)
        {
            Assert.Equal(expectedValue, @enum.GetSql());
        }

        [Fact]
        public void GetSql_EnumDoesNotHaveSqlPartialAttribute_ThrowsException()
        {
            var @enum = TestEnum.Test;
            var exception = Assert.Throws<MissingSqlPartialException>(() => @enum.GetSql());
            Assert.Equal(String.Format(MissingSqlPartialException.ERROR_MESSAGE, 
                                       nameof(SqlPartialAttribute),
                                       @enum.GetType().Name,
                                       @enum.ToString()), 
                         exception.Message);
        }

        enum TestEnum
        {
            Test
        }
    }
}
