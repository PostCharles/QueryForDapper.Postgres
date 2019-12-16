using Moq;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]
namespace Test.Fixtures
{
    public static class QueryMocks
    {
        public static Mock<IQuery> Query { get; } = new Mock<IQuery>();
    }
}
