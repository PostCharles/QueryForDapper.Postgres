using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using QueryForDapper.Postgres.Models;
using System.Text;
using System.Threading.Tasks;
using Test.Models;
using Xunit;

namespace Test
{
    [Trait("sut",nameof(LinqExpressionExtensions))]
    public class LinqExpressionExtensionsTest
    {
        [Fact]
        public void GetMemberInfo_Bool_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.BoolName;
            Assert.Equal(nameof(PropertyTest.BoolName),
                         expression.GetMemberInfo().Name);
                        
        }

        [Fact]
        public void GetMemberInfo_Byte_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.ByteName;
            Assert.Equal(nameof(PropertyTest.ByteName),
                         expression.GetMemberInfo().Name);

        }

        [Fact]
        public void GetMemberInfo_Class_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.ClassName;
            Assert.Equal(nameof(PropertyTest.ClassName),
                         expression.GetMemberInfo().Name);

        }

        [Fact]
        public void GetMemberInfo_DateTime_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.DateTimeName;
            Assert.Equal(nameof(PropertyTest.DateTimeName),
                         expression.GetMemberInfo().Name);

        }

        [Fact]
        public void GetMemberInfo_Decimal_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.DecimalName;
            Assert.Equal(nameof(PropertyTest.DecimalName),
                         expression.GetMemberInfo().Name);

        }

        [Fact]
        public void GetMemberInfo_Double_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.DoubleName;
            Assert.Equal(nameof(PropertyTest.DoubleName),
                         expression.GetMemberInfo().Name);

        }

        [Fact]
        public void GetMemberInfo_Int_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.IntName;
            Assert.Equal(nameof(PropertyTest.IntName),
                         expression.GetMemberInfo().Name);

        }

        [Fact]
        public void GetMemberInfo_String_ReturnsNameOfProperty()
        {
            Expression<Func<PropertyTest, object>> expression = (PropertyTest t) => t.StringName;
            Assert.Equal(nameof(PropertyTest.StringName),
                         expression.GetMemberInfo().Name);

        }

        [Fact]
        public void GetDapperParameter_ReturnsParameterizedNameFromGetExpression()
        {
            Expression<Func<Left,object>> expression = l => l.LeftId;

            Assert.Equal($"@{expression.GetMemberInfo().Name}",
                         expression.GetDapperParameter());
        }

    }
}
