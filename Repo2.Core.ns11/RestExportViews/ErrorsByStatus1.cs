using System.Collections.Generic;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.RestExportViews
{
    public class ErrorsByStatus1 : R2ErrorTicket, IRestExportView
    {
        public string DisplayPath => "errors-by-status-1";


        public List<string> CastArguments(object[] args)
            //=> new List<string> { args[0].ToString() };
            => new List<string>();
    }
}
