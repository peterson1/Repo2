using System.Threading.Tasks;
using PropertyChanged;
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

            var checkr = UploaderTab.ConfigChecker;

            checkr.PackagePathChanged += async (a, b) =>
            {
                await PreviousVerTab.GetVersions(checkr.PackageFileName);
            };
        }

        public UploaderTabVM     UploaderTab     { get; private set; }
        public PreviousVerTabVM  PreviousVerTab  { get; private set; }
    }
}
