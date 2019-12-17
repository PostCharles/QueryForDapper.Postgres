using QueryForDapper.Postgres.NamingSchemes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.NamingSchemeTest
{
    [Trait("sut", nameof(SnakeCaseScheme))]
    public class SnakeCaseSchemeTest
    {
        private SnakeCaseScheme _sut;

        public SnakeCaseSchemeTest()
        {
            _sut = new SnakeCaseScheme();
        }

        [Theory]
        [InlineData("Column", "column")]
        [InlineData("ColumnTwo", "column_two")]
        [InlineData("ColumnNumberThree", "column_number_three")]
        public void RenameColumn_ReturnsSnakeCasedColumn(string column, string expectedResult)
        {
            Assert.Equal(expectedResult, _sut.RenameColumn(column));
        }

        [Theory]
        [InlineData("Table", "tables")]
        [InlineData("TableTwo", "table_twos")]
        [InlineData("TableNumberThree", "table_number_threes")]
        public void RenameTable_ReturnsPluralizedSnakeCaseTable(string table, string expectedResult)
        {
            Assert.Equal(expectedResult, _sut.RenameTable(table));
        }
    }
}
