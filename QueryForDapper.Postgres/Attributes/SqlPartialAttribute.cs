using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Attributes
{
    public class SqlPartialAttribute : Attribute
    {
        public string Sql { get; }
        public SqlPartialAttribute(string sql)
        {
            Sql = sql;
        }

        
    }
}
