using QueryForDapper.Postgres.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QueryForDapper.Postgres.NamingSchemes
{
    public class SnakeCaseScheme : INamingScheme
    {
        public string RenameColumn(string column)
        {
            return Regex.Match(column, @"^_+") + Regex.Replace(column, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public string RenameTable(string table)
        {
            return $"{RenameColumn(table)}s";
        }
    }
}
