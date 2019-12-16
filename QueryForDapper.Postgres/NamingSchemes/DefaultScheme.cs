using QueryForDapper.Postgres.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.NamingSchemes
{
    public class DefaultScheme : INamingScheme
    {

        public string RenameColumn(string column)
        {
            return column;
        }

        public string RenameTable(string table)
        {
            return $"{table}s";
        }
    }
}
