using QueryForDapper.Postgres.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.NamingSchemes
{
    public class SnakeCaseScheme : INamingScheme
    {
        public string RenameColumn(string column)
        {
            throw new NotImplementedException();
        }

        public string RenameTable(string table)
        {
            throw new NotImplementedException();
        }
    }
}
