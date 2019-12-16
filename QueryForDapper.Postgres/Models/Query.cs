using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Extensions;
using QueryForDapper.Postgres.Models.Keywords;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public class Query : IQuery
    {

        private List<OrderBy> _orderBys;
        public IReadOnlyList<OrderBy> OrderBys
        {
            get => _orderBys;
        }

        private List<Join> _joins;
        public IReadOnlyList<Join> Joins
        {
            get => _joins;
        }

        private List<Select> _selects;
        public IReadOnlyList<Select> Selects
        {
            get => _selects;
        }

        private List<Where> _wheres;
        public IReadOnlyList<Where> Wheres
        {
            get => _wheres;
        }

        public string LimitParameter { get; private set; }
        public string OffsetParameter { get; private set; }
        public string StartTable { get;}

        public Query(string startTable)
        {
            _joins = new List<Join>();
            _orderBys = new List<OrderBy>();
            _selects = new List<Select>();
            _wheres = new List<Where>();

            StartTable = startTable.ToTableName();
        }

        public static QueryConfiguration ConfigureTo() { return QueryConfiguration.Current; }
        public static Query FromTable<T>() { return new Query(typeof(T).Name); }



        public void AddLimitParameter(string parameter)
        {
            LimitParameter = $"@{parameter}";
        }

        public void AddOffsetParameter(string parameter)
        {
            OffsetParameter = $"@{parameter}";
        }

        public void AddOrderBy(MemberInfo column, Type table, Order order)
        {
            _orderBys.Add(new OrderBy(column.ToColumnName(), table.ToTableName(), order));
        }

        public void AddJoin(MemberInfo column, Type table, JoinType joinType)
        {
            _joins.Add(new Join(column.ToColumnName(), table.ToTableName(), joinType));
        }

        public void AddJoin(string column, Type table, JoinType joinType)
        {
            _joins.Add(new Join(column, table.ToTableName(), joinType));
        }

        public void AddSelect(MemberInfo column, Type table)
        {
            _selects.Add(new Select(column.ToColumnName(), table.ToTableName()));
        }
        public void AddSelect(string column, Type table)
        {
            _selects.Add(new Select(column, table.ToTableName()));
        }

        public void AddWhere(MemberInfo column, Type table, string predicate, Operator @operator)
        {
            _wheres.Add(new Where(column.ToColumnName(), table.ToTableName(), 
                                  @operator, 
                                  predicate));
        }
    }
}
