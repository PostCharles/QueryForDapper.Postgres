using QueryForDapper.Postgres.NamingSchemes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.NamingSchemeTest
{
    [Trait("sut", nameof(DefaultScheme))]
    public class DefaultSchemeTest
    {
        private DefaultScheme _sut;

        public DefaultSchemeTest()
        {
            _sut = new DefaultScheme();
        }

        [Theory]
        [InlineData("Column")]
        [InlineData("ColumnTwo")]
        [InlineData("ColumnNumberThree")]
        public void RenameColumn_ReturnsColumnName(string column)
        {
            Assert.Equal(column, _sut.RenameColumn(column));
        }

        [Theory]
        [InlineData("Table")]
        [InlineData("TableTwo")]
        [InlineData("TableNumberThree")]
        public void RenameTable_PluralizesTableName(string table)
        {
            Assert.Equal($"{table}s", _sut.RenameTable(table));
        }
     }
}
