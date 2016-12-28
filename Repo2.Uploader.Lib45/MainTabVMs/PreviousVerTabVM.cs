using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace Repo2.Uploader.Lib45.MainTabVMs
{
    [ImplementPropertyChanged]
    public class PreviousVerTabVM
    {
        internal async Task GetVersions(string packageFileName)
        {
            Title = "finding previous versions ...";
            await Task.Delay(1000 * 2);
            Title = $"Previous Versions ({2})";
        }


        public string Title { get; private set; }
    }
}
