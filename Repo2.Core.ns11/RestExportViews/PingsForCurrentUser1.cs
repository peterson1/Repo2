using System;
using System.Collections.Generic;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.RestExportViews
{
    public class PingsForCurrentUser1 : R2Ping, IRestExportView
    {
        public string DisplayPath => "pings-for-current-user-1";


        public string  PackageFilename  { get; set; }


        public List<string> CastArguments(object[] args)
            => new List<string>();


        public void PostProcess(object obj)
        {
        }
    }
}
