namespace Repo2.Core.ns11.Databases
{
    public class ConnectString
    {
        public static string LiteDB(string filePath, string mode = "Exclusive", bool journal = false)
            => $"Filename={filePath};Mode={mode};Journal={journal};";
    }
}
