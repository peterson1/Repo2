using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Drupal8.Attributes;

namespace Repo2.Core.ns11.DomainModels
{
    public class R2Package : D8NodeBase
    {
        public override string D8TypeName => "package";

        public R2Package()
        {
        }

        public R2Package(string filename)
        {
            Filename = filename;
        }

        [ContentTitle]      public string  Filename  { get; set; }
        [_("package_hash")] public string  Hash      { get; set; }

        public string  LocalDir    { get; set; }
        public bool    FileFound   { get; set; }
    }
}
