using System.Collections.Generic;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Drupal8.Attributes;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.DomainModels
{
    public class R2PackagePart : D8NodeBase
    {
        public override string D8TypeName => "package_part";

        [_("package_filename")] public string   PackageFilename  { get; set; }
        [_("package_hash")]     public string   PackageHash      { get; set; }
        [_("part_hash")]        public string   PartHash         { get; set; }
        [_("part_number")]      public int      PartNumber       { get; set; }
        [_("total_parts")]      public int      TotalParts       { get; set; }
        [ContentTitle]          public string   Description      => GetDescription();


        private string GetDescription()
            => $"{PackageFilename} part {PartNumber} of {TotalParts}";


        public bool IsValid()
        {
            var errMsgs = new List<string>();
            return IsValid(ref errMsgs);
        }


        public bool IsValid(ref List<string> errors)
        {
            var typ = $"‹{GetType().Name}›";

            if (PackageFilename.IsBlank())
                errors.Add($"{typ}.{nameof(PackageFilename)} should not be blank.");

            if (PackageHash.IsBlank())
                errors.Add($"{typ}.{nameof(PackageHash)} should not be blank.");

            if (PartHash.IsBlank())
                errors.Add($"{typ}.{nameof(PartHash)} should not be blank.");

            if (PartNumber < 1)
                errors.Add($"{typ}.{nameof(PartNumber)} should be greater than zero; but found [{PartNumber}].");

            if (TotalParts < 1)
                errors.Add($"{typ}.{nameof(TotalParts)} should be greater than zero; but found [{TotalParts}].");

            if (PartNumber > TotalParts)
                errors.Add($"{typ}.{nameof(PartNumber)} [{PartNumber}] should NOT be greater than {nameof(TotalParts)} [{TotalParts}].");

            return errors.Count == 0;            
        }
    }
}
