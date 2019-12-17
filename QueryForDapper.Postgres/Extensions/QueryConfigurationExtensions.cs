using QueryForDapper.Postgres.Exceptions;
using QueryForDapper.Postgres.Extensions;
using QueryForDapper.Postgres.Interfaces;
using QueryForDapper.Postgres.NamingSchemes;
using System;
using System.Linq.Expressions;

namespace QueryForDapper.Postgres.Models
{
    public static class QueryConfigurationExtensions
    {

        public static QueryConfiguration NameTablesWith(this QueryConfiguration config, Func<string, string> method)
        {
            QueryConfiguration.Current.TableNameMethod = method;

            return config;
        }

        public static QueryConfiguration NameColumnsWith(this QueryConfiguration config, Func<string, string> method)
        {
            QueryConfiguration.Current.ColumnNameMethod = method;

            return config;
        }

        public static QueryConfiguration UseDefaultNaming(this QueryConfiguration config)
        {
            config.AssignNameMethodsFromScheme(new DefaultScheme());

            return config;
        }

        public static QueryConfiguration UsePassthroughNaming(this QueryConfiguration config)
        {
            config.AssignNameMethodsFromScheme(new PassthroughScheme());

            return config;
        }

        public static QueryConfiguration UseSnakeCaseNaming(this QueryConfiguration config)
        {
            config.AssignNameMethodsFromScheme(new SnakeCaseScheme());

            return config;
        }

        public static QueryConfiguration UseCustomNamingScheme(this QueryConfiguration config, INamingScheme scheme)
        {
            config.AssignNameMethodsFromScheme(scheme);

            return config;
        }

        public static QueryConfiguration UseColumnAttributeNames(this QueryConfiguration config)
        {
            config.ShouldUseColumnAttributes = true;

            return config;
        }

        public static QueryConfiguration UseTableAttributeNames(this QueryConfiguration config)
        {
            config.ShouldUseTableAttributes = true;

            return config;
        }

        public static QueryConfiguration MapManyToMany<TLeft,TJoin,TRight>(this QueryConfiguration config, Expression<Func<TJoin,object>> leftKey, Expression<Func<TJoin, object>> rightKey)
        {

            try
            {
                QueryConfiguration.Current.JoinMaps.Add(new JoinMap(typeof(TLeft),
                                                            typeof(TJoin),
                                                            typeof(TRight),
                                                            leftKey.Body.GetMemberInfo().ToColumnName(),
                                                            rightKey.Body.GetMemberInfo().ToColumnName()));
                return config;
            }
            catch (NameMethodNotSetException ex)
            {

                throw new ConfigurationOrderException(nameof(MapManyToMany),ex);
            }
        }

        public static QueryConfiguration MapManyToMany<TLeft, TJoin, TRight>(this QueryConfiguration config, string leftKeyColumn, string rightKeyColumn)
        {
            QueryConfiguration.Current.JoinMaps.Add(new JoinMap(typeof(TLeft),
                                                                typeof(TJoin),
                                                                typeof(TRight),
                                                                leftKeyColumn,
                                                                rightKeyColumn));
            return config;
        }

        public static QueryConfiguration DefineColumnName<T>(this QueryConfiguration config, Expression<Func<T, object>> columnSelector, string columnName)
        {
            QueryConfiguration.Current.ColumnDefinitions.Add(new ColumnDefinition(typeof(T),
                                                                                  columnSelector.Body.GetMemberInfo().Name,
                                                                                  columnName));
            return config;
        }

        public static QueryConfiguration DefineColumnUsingColumnNaming<T>(this QueryConfiguration config, Expression<Func<T, object>> columnSelector, string columnName)
        {
            try
            {
                DefineColumnName(config, columnSelector, columnName.ToColumnName());

            }
            catch (NameMethodNotSetException ex)
            {
                throw new ConfigurationOrderException(nameof(DefineColumnUsingColumnNaming), ex);
            }
            return config;
        }

        public static QueryConfiguration DefineTableName<T>(this QueryConfiguration config, string tableName)
        {
            QueryConfiguration.Current.TableDefinitions.Add(typeof(T), tableName);

            return config;
        }

        public static QueryConfiguration DefineTableUsingTableNaming<T>(this QueryConfiguration config, string tableName)
        {
            try
            {
                DefineTableName<T>(config, tableName.ToTableName());

            }
            catch (NameMethodNotSetException ex)
            {
                throw new ConfigurationOrderException(nameof(DefineTableUsingTableNaming), ex);
            }
            return config;
        }
    }
}
