using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.Databases
{
    public class ConnectString
    {
        public static string LiteDB(string filePath, string mode = "Exclusive", bool journal = false)
            => $"Filename={filePath};Mode={mode};Journal={journal};";


        public static string MsAccess(string filePath, string password = null)
        {
            var cs = $"Provider=Microsoft.Jet.OLEDB.4.0;"
                   + $"Data Source={filePath};";

            if (!password.IsBlank())
                cs += $"Jet OLEDB:Database Password={password};";

            return cs;
        }
    }
}
