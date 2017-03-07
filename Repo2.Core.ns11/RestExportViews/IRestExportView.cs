using System.Collections.Generic;

namespace Repo2.Core.ns11.RestExportViews
{
    public interface IRestExportView
    {
        string DisplayPath { get; }

        List<string> CastArguments(object[] args);

        void PostProcess(object obj);
    }
}
