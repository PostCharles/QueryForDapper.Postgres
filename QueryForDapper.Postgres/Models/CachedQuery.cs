using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public class CachedQuery
    {
		public IQuery Query { get; private set; }
		public string QueryId { get; private set; }
		public Type QueryType { get; private set; }
		public string Statement { get; private set; }


		public CachedQuery(IQuery query, string queryId)
		{
			QueryId = queryId;
			Query = query.GetShallowClone();
			
			Statement = Query.ToStatement();

		}

		public CachedQuery(IQuery query, string queryId, Type queryType) : this(query,queryId)
		{
			QueryType = queryType;
		}


		public bool IsMatch(string queryId)
		{
			return (QueryType is null && queryId == QueryId);
		}

		public bool IsMatch(string queryId, Type queryType)
		{
			return (QueryId == queryId && QueryType == queryType);
		}
    }
}
