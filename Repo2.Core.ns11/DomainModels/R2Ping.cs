using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Drupal8.Attributes;

namespace Repo2.Core.ns11.DomainModels
{
    public class R2Ping : D8NodeBase
    {
        public override string D8TypeName => "ping";

        [ContentTitle]         public string  Title => $"Ping for {WindowsAccount}";
        [_("windows_account")] public string  WindowsAccount  { get; set; }
        [_("package_hash")]    public string  PingerHash      { get; set; }
    }
}
