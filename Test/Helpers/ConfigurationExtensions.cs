using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Models
{
    public static class ConfigurationExtensions
    {
        public static void ResetConfigurations(this QueryConfiguration config)
        {
            typeof(QueryConfiguration).GetMethod("Reset", BindingFlags.Static | BindingFlags.NonPublic)
                                      .Invoke(config,null);
        }
    }
}
