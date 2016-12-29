using System.Threading.Tasks;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Uploader.Lib45.MainTabVMs;

namespace Repo2.Uploader.Lib45
{
    [ImplementPropertyChanged]
    public class MainWindowVM
    {
        public MainWindowVM(UploaderTabVM uploaderTabVM,
                            PreviousVerTabVM previousVerTabVM)
        {
            UploaderTab    = uploaderTabVM;
            PreviousVerTab = previousVerTabVM;
            Tabs           = new Observables<object> { UploaderTab, PreviousVerTab };

            var checkr = UploaderTab.ConfigChecker;

            checkr.PackagePathChanged += async (a, b) =>
            {
                await PreviousVerTab.GetVersions(checkr.PackageFileName);
            };
        }

        public Observables<object>  Tabs            { get; private set; }
        public UploaderTabVM        UploaderTab     { get; private set; }
        public PreviousVerTabVM     PreviousVerTab  { get; private set; }
    }
}
