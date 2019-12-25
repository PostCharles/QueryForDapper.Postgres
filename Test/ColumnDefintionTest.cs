using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.TestModels;
using Xunit;

namespace Test
{
    [Trait("sut",nameof(ColumnDefinition))]
    public class ColumnDefintionTest
    {
        [Theory]
        [InlineData(nameof(Table.TableId), typeof(Table), false)]
        [InlineData(nameof(UsingType.Declared), typeof(DeclaringType), false)]
        [InlineData(nameof(UsingType.Declared), typeof(UsingType), true)]
        public void IsMatch_ReturnsTrueIfMatch(string column, Type table, bool expectedResult)
        {
            var sut = new ColumnDefinition(typeof(UsingType), nameof(UsingType.Declared), "DefineColumn");

            Assert.Equal(expectedResult, sut.IsMatch(column, table));
        }
    }
}
