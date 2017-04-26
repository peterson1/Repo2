using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using System.Collections.Generic;

namespace Repo2.Core.ns11.RestExportViews
{
    public class UserInfoByAuthToken : IRestExportView
    {
        public string DisplayPath => "verify-auth-token";


        public int      UserId    { get; set; }
        public string   Username  { get; set; }
        public string   AllRoles  { get; set; }


        public List<string> Roles => AllRoles.SplitTrim(",");


        public List<string> CastArguments(object[] args)
        {
            if (args.Length < 1)
                throw Fault.BadArg(nameof(args), "have at least one item");

            return new List<string> { $"{args[0]}" };
        }


        public void PostProcess()
        {
        }
    }
}
