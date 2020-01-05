using KellermanSoftware.CompareNetObjects;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Helpers;
using Test.TestModels;
using Xunit;

namespace Test.QueryCacheTests
{
    [Trait("sut","cachedQuery")]
    public class CachedQueryTest : BaseDispose
    {
        private IQuery _queryToCache;

        public CachedQueryTest()
        {
            QueryConfiguration.Current.UseDefaultNaming();
            _queryToCache = Query.FromTable<Table>()
                                 .Select<Table>(j => j.TableId);
        }

        [Fact]
        public void Constructor_ConvertsQueryToStatementCache()
        {
            var sut = new CachedQuery(_queryToCache, "");
            _queryToCache.OrderBy<Table>(j => j.TableId);

            Assert.NotEqual(sut.Statement, _queryToCache.ToStatement());
        }

        [Fact]
        public void Constructor_CachedQueryIsCloned()
        {
            var sut = new CachedQuery(_queryToCache, "");
            
            Assert.NotEqual(_queryToCache, sut.Query);

            var comparer = new CompareLogic();
            
            Assert.True(comparer.Compare(_queryToCache, sut.Query).AreEqual);
        }

        [Fact]
        public void IsMatchById_CachedQueryHasQueryType_ReturnsFalse()
        {
            var id = "QueryId";
            var sut = new CachedQuery(_queryToCache, id, typeof(Table));

            Assert.False(sut.IsMatch(id));
        }

        [Fact]
        public void IsMatchById_IdMismatch_ReturnsFalse()
        {
            var sut = new CachedQuery(_queryToCache, "id");

            Assert.False(sut.IsMatch("notId"));
        }

        [Fact]
        public void IsMatchById_Match_ReturnsTrue()
        {
            var id = "id";
            
            var sut = new CachedQuery(_queryToCache, "id");

            Assert.True(sut.IsMatch(id));
        }

        [Fact]
        public void IsMatchByQueryType_NoQueryType_ReturnsFalse()
        {
            var id = "id";

            var sut = new CachedQuery(_queryToCache, id);

            Assert.False(sut.IsMatch(id, typeof(Table)));
        }

        [Fact]
        public void IsMatchByQueryType_IdMismatch_ReturnsFalse()
        {
            var type = typeof(Table);
            var sut = new CachedQuery(_queryToCache, "id", type);

            Assert.False(sut.IsMatch("nonId", typeof(Table)));
        }

        [Fact]
        public void IsMatchByQueryType_QueryTypeMisMatch_ReturnsFalse()
        {
            var id = "id";

            var sut = new CachedQuery(_queryToCache, id, typeof(Left));

            Assert.False(sut.IsMatch(id, typeof(Right)));
        }


        [Fact]
        public void IsMatchByQueryType_IdAnQueryTypeMatch_ReturnsTrue()
        {
            var id = "id";
            var type = typeof(Table);
            var sut = new CachedQuery(_queryToCache, id, type);

            Assert.True(sut.IsMatch(id, type));
        }
    }
}
