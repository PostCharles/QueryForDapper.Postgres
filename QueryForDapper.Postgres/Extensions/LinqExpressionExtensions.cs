using QueryForDapper.Postgres.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public static class LinqExpressionExtensions
    {
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            Expression member = GetMemberExpression(expression);

            return ((MemberExpression)member).Member;

        }

        private static Expression GetMemberExpression(Expression expression)
        {
            return expression.NodeType switch
            {
                ExpressionType.Convert => ((UnaryExpression)expression).Operand,
                ExpressionType.MemberAccess => expression,
                ExpressionType.Lambda => GetMemberExpression( ((LambdaExpression)expression).Body ),
                _ => throw new UnableToRetrieveExpressionPropertyName()
            };
        }

        public static string GetDapperParameter(this Expression expression)
        {
            return $"@{expression.GetMemberInfo().Name}";
        }
    }
}
