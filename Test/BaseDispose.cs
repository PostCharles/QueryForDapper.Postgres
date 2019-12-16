using QueryForDapper.Postgres.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class BaseDispose : IDisposable
    {
        public virtual void Dispose()
        {
            QueryConfiguration.Current.ResetConfigurations();
        }
    }
}
