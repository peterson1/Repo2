namespace Repo2.Core.ns11.DomainModels
{
    public class R2Package
    {
        public R2Package(string filename)
        {
            Filename = filename;
        }

        public string  Filename    { get; set; }
        public string  LocalDir    { get; set; }
        public string  LocalHash   { get; set; }
        public string  RemoteHash  { get; set; }
    }
}
