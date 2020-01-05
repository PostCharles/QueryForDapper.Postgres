using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Interfaces
{
    public interface IQueryCacheService
    {
        void AddIfNew(string queryId, Func<IQuery> queryToCache);
        void AddIfNew<T>(string queryId, Func<IQuery> queryToCache);

        IQuery GetQuery(string queryId);
        IQuery GetQuery<T>(string queryId);

        string GetStatement(string queryId);
        string GetStatement<T>(string queryId);
    }
}
