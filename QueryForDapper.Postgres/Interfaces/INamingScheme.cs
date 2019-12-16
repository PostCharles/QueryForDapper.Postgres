using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.Interfaces
{
    public interface INamingScheme
    {
        string RenameColumn(string column);
        string RenameTable(string table);
    }
}
