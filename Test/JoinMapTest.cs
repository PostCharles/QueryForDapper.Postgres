using QueryForDapper.Postgres.Exceptions;
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
    [Trait("sut",nameof(JoinMap))]
    public class JoinMapTest
    {
        private JoinMap _sut;

        public JoinMapTest()
        {
            _sut = new JoinMap(leftTable: typeof(Left), 
                               joinTable: typeof(Join), 
                               rightTable: typeof(Right), 
                               leftColumn: nameof(Join.LeftId), 
                               rightColumn: nameof(Join.RightId));
        }

        [Theory]
        [InlineData(typeof(Left),typeof(Join))]
        [InlineData(typeof(Join), typeof(Right))]
        [InlineData(typeof(Table), typeof(Join))]
        public void CanJoin_MapDoesNotContainBothLeftAndRight_ReturnsFalse(Type leftType, Type rightType)
        {
            Assert.False(_sut.CanJoin(leftType, rightType));
        }

        [Theory]
        [InlineData(typeof(Left), typeof(Right))]
        [InlineData(typeof(Right), typeof(Left))]
        public void CanJoin_MapContainBothLeftAndRight_ReturnsTrueRegardlessOfOrder(Type leftType, Type rightType)
        {
            Assert.True(_sut.CanJoin(leftType, rightType));
        }

        [Theory]
        [InlineData(typeof(Left), nameof(Join.LeftId))]
        [InlineData(typeof(Right), nameof(Join.RightId))]
        public void GetLeft_ReturnsJoinTableAndMatchingKey(Type tableType, string column)
        {
            var left = _sut.GetLeft(tableType);
            Assert.Equal(typeof(Join), left.Table);
            Assert.Equal(column, left.Column);
        }

        [Theory]
        [InlineData(typeof(Left), nameof(Join.LeftId))]
        [InlineData(typeof(Right), nameof(Join.RightId))]
        public void GetRight_ReturnsMatchingTableAndMatchingKey(Type tableType, string column)
        {
            var right = _sut.GetRight(tableType);
            Assert.Equal(tableType, right.Table);
            Assert.Equal(column, right.Column);
        }


    }
}
