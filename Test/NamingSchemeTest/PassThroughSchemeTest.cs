﻿using QueryForDapper.Postgres.NamingSchemes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test.NamingSchemeTest
{
    [Trait("sut", nameof(PassthroughScheme))]
    public class PassThroughSchemeTest
    {
        private PassthroughScheme _sut;

        public PassThroughSchemeTest()
        {
            _sut = new PassthroughScheme();
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
        public void RenameTable_ReturnsTableName(string table)
        {
            Assert.Equal(table, _sut.RenameTable(table));
        }
    }
}
