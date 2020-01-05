using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Interfaces;
using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Services
{
    public class ConcurrentQueryCacheService : IQueryCacheService
    {
        private ConcurrentDictionary<string,CachedQuery> _cache;

        public ConcurrentQueryCacheService()
        {
            _cache = new ConcurrentDictionary<string, CachedQuery>();
        }

        public void AddIfNew(string queryId, Func<IQuery> queryToCache)
        {
            if (! _cache.TryGetValue(queryId, out var cachedQuery))
            {
                var query = queryToCache.Invoke();
                _cache.TryAdd(queryId, new CachedQuery(query, queryId) );
            }
        }

        public void AddIfNew<T>(string queryId, Func<IQuery> queryToCache)
        {
            if (!_cache.TryGetValue(queryId, out var cachedQuery))
            {
                var type = typeof(T);
                var query = queryToCache.Invoke();
                _cache.TryAdd(GetTypedQueryId(queryId,type), 
                              new CachedQuery(query, queryId, type));
            }
        }

        public IQuery GetQuery(string queryId)
        {
            if (_cache.TryGetValue(queryId, out var cache) )
            {
                return cache.Query;
            }

            throw new CachedQueryNotFound(queryId);
        }

        public IQuery GetQuery<T>(string queryId)
        {
            if (_cache.TryGetValue( GetTypedQueryId(queryId,typeof(T)), out var cache))
            {
                return cache.Query;
            }

            throw new CachedQueryNotFound(queryId, typeof(T));
        }

        public string GetStatement(string queryId)
        {
            if (_cache.TryGetValue(queryId, out var cache))
            {
                return cache.Statement;
            }

            throw new CachedQueryNotFound(queryId);
        }

        public string GetStatement<T>(string queryId)
        {
            if (_cache.TryGetValue(GetTypedQueryId(queryId, typeof(T)), out var cache))
            {
                return cache.Statement;
            }

            throw new CachedQueryNotFound(queryId, typeof(T));
        }

        private string GetTypedQueryId(string queryId, Type queryType)
        {
            return $"{queryId}_{queryType.Name}";
        }
    }
}
