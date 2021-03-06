﻿using QueryForDapper.Postgres.Enums;
using QueryForDapper.Postgres.Models.Keywords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public interface IQuery
    {
        IReadOnlyList<OrderBy> OrderBys { get; }
        IReadOnlyList<Join> Joins { get; }
        IReadOnlyList<Select> Selects { get; }
        IReadOnlyList<Where> Wheres { get; }
        string LimitParameter { get; }
        string OffsetParameter { get; }
        string StartTable { get; }

        void AddJoin(MemberInfo column, Type table, JoinType joinType);
        void AddJoin(string column, Type table, JoinType joinType);
        void AddJoin(string column, Type table, string columnLeft, Type tableLeft, JoinType joinType);
        void AddJoin(MemberInfo column, Type table, MemberInfo columnLeft, Type tableLeft, JoinType joinType);
        void AddOrderBy(MemberInfo column, Type table, Order order);
        void AddSelect(MemberInfo column, Type table, string @as = null);
        void AddSelect(string column, Type table, string @as = null);
        void AddWhere(MemberInfo column, Type table, string predicate, Operator @operator);
        void AddLimitParameter(string parameter);
        void AddOffsetParameter(string parameter);
        IQuery GetShallowClone();
    }
}
