using QueryForDapper.Postgres.Models.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.KeywordTests
{
    public class SelectTest
    {
        [Fact]
        public void GetAs_AsIsNull_ReturnsStringEmpty()
        {
            var select = new Select("column", "table", null);

            Assert.Empty(select.GetAsSqlPartial());
        }

        [Fact]
        public void GetAs_AsSet_ReturnsAsPartial()
        {
            var @as = "Stuff";
            var select = new Select("column", "table", @as);

            Assert.Equal($" AS {@as}", select.GetAsSqlPartial());
        }
    }
}
