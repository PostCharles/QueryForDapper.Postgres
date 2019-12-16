using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public class QueryConfiguration
    {
        public static QueryConfiguration Current { get { return _lazy.Value; } }
        private static Lazy<QueryConfiguration> _lazy = new Lazy<QueryConfiguration>(() => new QueryConfiguration());

        private Func<string, string> _columnNameMethod;
        public Func<string, string> ColumnNameMethod
        {
            get => _columnNameMethod ?? throw new NameMethodNotSetException(nameof(ColumnNameMethod));
            set => _columnNameMethod = (_columnNameMethod == null) ? value : throw new NameMethodSetException(nameof(ColumnNameMethod));
        }

        private Func<string, string> _tableNameMethod;
        public Func<string, string> TableNameMethod
        {
            get => _tableNameMethod ?? throw new NameMethodNotSetException(nameof(TableNameMethod));
            set => _tableNameMethod = (_tableNameMethod == null) ? value : throw new NameMethodSetException(nameof(TableNameMethod));
        }

        public ICollection<ColumnDefinition> ColumnDefinitions = new List<ColumnDefinition>();
        public ICollection<JoinMap> JoinMaps { get; } = new List<JoinMap>();

        public bool ShouldUseColumnAttributes { get; set; } = false;


        private QueryConfiguration()
        {
        }

        private static void Reset() => _lazy = new Lazy<QueryConfiguration>(() => new QueryConfiguration());

        public void AssignNameMethodsFromScheme(INamingScheme scheme)
        {
            ColumnNameMethod = scheme.RenameColumn;
            TableNameMethod = scheme.RenameTable;
        }
    }
}