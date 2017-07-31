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


        public static string SqlServer(string serverAddress, 
                                       string databaseName,
                                       string userId,
                                       string password)
        {
            var url = $"Data Source={serverAddress}";
            var lib = $"Network Library=DBMSSOCN";
            var db  = $"Initial Catalog={databaseName}";
            var usr = $"User id={userId}";
            var pwd = $"Password={password}";

            return  $"{url};{lib};{db};{usr};{pwd};";
        }
    }
}
