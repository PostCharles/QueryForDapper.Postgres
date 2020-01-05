using KellermanSoftware.CompareNetObjects;
using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Models;
using QueryForDapper.Postgres.Services;
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
    [Trait("sut", nameof(ConcurrentQueryCacheService))]
    public class ConcurrentQueryCacheServiceTest : BaseDispose
    {
        private const string QUERY_ID = "queryId";

        private ConcurrentQueryCacheService _sut;

        public ConcurrentQueryCacheServiceTest()
        {
            QueryConfiguration.Current.UseDefaultNaming();
            _sut = new ConcurrentQueryCacheService();
        }

        [Fact]
        public void AddIfNew_NewQueryForId_AddsQueryToCache()
        {
            var query = Query.FromTable<Table>();

            _sut.AddIfNew(QUERY_ID, () => query);

            var cachedQuery = _sut.GetQuery(QUERY_ID);
            
            Assert.True(new CompareLogic().Compare(query, cachedQuery).AreEqual);
        }

        [Fact]
        public void AddIfNew_AlreadyCached_DoesNotInvokeDelegate()
        {
            Func<IQuery> query = () => Query.FromTable<Table>();

            _sut.AddIfNew(QUERY_ID, query);
            _sut.AddIfNew(QUERY_ID, query);
            _sut.AddIfNew(QUERY_ID, query);

            Assert.Single(query.GetInvocationList());
        }

        [Fact]
        public void AddIfNewByType_NewQueryById_AddsQueryToCacheByCombinationIdAndType()
        {
            var leftQuery = Query.FromTable<Left>();
            var rightQuery = Query.FromTable<Right>();

            _sut.AddIfNew<Left>(QUERY_ID, () => leftQuery);
            _sut.AddIfNew<Right>(QUERY_ID, () => rightQuery);

            var cachedLeftQuery = _sut.GetQuery<Left>(QUERY_ID);
            Assert.True(new CompareLogic().Compare(leftQuery, cachedLeftQuery).AreEqual);

            var cachedRightQuery = _sut.GetQuery<Right>(QUERY_ID);
            Assert.True(new CompareLogic().Compare(rightQuery, cachedRightQuery).AreEqual);
        }

        [Fact]
        public void AddIfNewByType_QueryAlreadyCached_DoesNotInvokeDelegate()
        {
            Func<IQuery> query = () => Query.FromTable<Table>();

            _sut.AddIfNew<Table>(QUERY_ID, query);
            _sut.AddIfNew<Table>(QUERY_ID, query);
            _sut.AddIfNew<Table>(QUERY_ID, query);

            Assert.Single(query.GetInvocationList());
        }

        [Fact]
        public void GetQuery_QueryNotFound_ThrowsException()
        {
            var exception = Assert.Throws<CachedQueryNotFound>(() =>_sut.GetQuery(QUERY_ID));
            Assert.Equal(String.Format(CachedQueryNotFound.ERROR_MESSAGE, QUERY_ID),
                         exception.Message);
        }

        [Fact]
        public void GetQuery_ReturnsCloneOfAddedQuery()
        {
            var query = Query.FromTable<Table>().Select<Table>(s => s.TableId);

            _sut.AddIfNew(QUERY_ID, () => query);

            var cachedQuery = _sut.GetQuery(QUERY_ID);
            Assert.NotEqual(query, cachedQuery);

            Assert.True(new CompareLogic().Compare(query, cachedQuery).AreEqual);
        }

        [Fact]
        public void GetQueryByType_QueryNotFound_ThrowsException()
        {
            var exception = Assert.Throws<CachedQueryNotFound>(() => _sut.GetQuery<Table>(QUERY_ID));
            Assert.Equal(String.Format(CachedQueryNotFound.ERROR_MESSAGE_TYPE, QUERY_ID, typeof(Table).Name),
                         exception.Message);
        }

        [Fact]
        public void GetQueryByType_ReturnsCloneOfRequestedQuery()
        {
            var leftQuery = Query.FromTable<Left>().Select<Left>(s => s.LeftId);
            var rightQuery = Query.FromTable<Right>().Select<Right>(s => s.RightId);

            _sut.AddIfNew<Left>(QUERY_ID, () => leftQuery);
            _sut.AddIfNew<Right>(QUERY_ID, () => rightQuery);

            var cachedLeftQuery = _sut.GetQuery<Left>(QUERY_ID);
            var cachedRightQuery = _sut.GetQuery<Right>(QUERY_ID);

            Assert.NotEqual(leftQuery, cachedLeftQuery);
            Assert.NotEqual(rightQuery, cachedRightQuery);

            Assert.True(new CompareLogic().Compare(leftQuery, cachedLeftQuery).AreEqual);
            Assert.True(new CompareLogic().Compare(rightQuery, cachedRightQuery).AreEqual);
        }

        [Fact]
        public void GetStatement_QueryNotFound_ThrowsException()
        {
            var exception = Assert.Throws<CachedQueryNotFound>(() => _sut.GetStatement(QUERY_ID));
            Assert.Equal(String.Format(CachedQueryNotFound.ERROR_MESSAGE, QUERY_ID),
                         exception.Message);
        }

        [Fact]
        public void GetStatement_ReturnsStatementGeneratedByQuery()
        {
            var query = Query.FromTable<Table>().Select<Table>(s => s.TableId);

            _sut.AddIfNew(QUERY_ID, () => query);

            Assert.Equal(query.ToStatement(), _sut.GetStatement(QUERY_ID));
        }

        [Fact]
        public void GetStatementByType_QueryNotFound_ThrowsException()
        {
            var exception = Assert.Throws<CachedQueryNotFound>(() => _sut.GetStatement<Table>(QUERY_ID));
            Assert.Equal(String.Format(CachedQueryNotFound.ERROR_MESSAGE_TYPE, QUERY_ID, typeof(Table).Name),
                         exception.Message);
        }

        [Fact]
        public void GetStatementByType_ReturnsStatementGeneratedByQuery()
        {
            var leftQuery = Query.FromTable<Left>().Select<Left>(s => s.LeftId);
            var rightQuery = Query.FromTable<Right>().Select<Right>(s => s.RightId);

            _sut.AddIfNew<Left>(QUERY_ID, () => leftQuery);
            _sut.AddIfNew<Right>(QUERY_ID, () => rightQuery);

            Assert.Equal(leftQuery.ToStatement(), _sut.GetStatement<Left>(QUERY_ID));
            Assert.Equal(rightQuery.ToStatement(), _sut.GetStatement<Right>(QUERY_ID));
        }
    }
}
