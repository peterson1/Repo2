using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Uploader.Lib45.Configuration;
using Repo2.Uploader.Lib45.MainTabVMs;
using System.ComponentModel;

namespace Repo2.Uploader.Lib45
{
    //[ImplementPropertyChanged]
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        public MainWindowVM(ConfigLoaderVM configLoaderVM,
                            UploaderTabVM uploaderTabVM,
                            PreviousVerTabVM previousVerTabVM)
        {
            ConfigLoader   = configLoaderVM;
            UploaderTab    = uploaderTabVM;
            PreviousVerTab = previousVerTabVM;
            Tabs           = new Observables<object> { UploaderTab, PreviousVerTab };

            ConfigLoader.ConfigLoaded += (a, cfg) =>
            {
                UploaderTab.ClientStatus = "";
                UploaderTab.PkgChecker.Clear();
            };

            ConfigLoader.PackageChanged += (a, pkg) =>
            {
                //PreviousVerTab.Clear();
                //PreviousVerTab.Filename = pkg?.Filename;
                PreviousVerTab.SetPackage(pkg?.Filename);
                UploaderTab.PkgUploader.DisableUpload();
                UploaderTab.PkgChecker.Package = pkg;

                if (UploaderTab.AccessChecker.CanWrite == true)
                    OnWriteAccessEnabled();
            };

            //ConfigLoader.PackageChanged += async (a, b) =>
            //{
            //    if (UploaderTab.AccessChecker.CanWrite == true)
            //        await PreviousVerTab.GetVersions(ConfigLoader.Package.Filename);
            //};

            UploaderTab.AccessChecker.WriteAccessEnabled += (a, b)
                => OnWriteAccessEnabled();
        }

        public Observables<object>  Tabs            { get; private set; }
        public ConfigLoaderVM       ConfigLoader    { get; private set; }
        public UploaderTabVM        UploaderTab     { get; private set; }
        public PreviousVerTabVM     PreviousVerTab  { get; private set; }


        private void OnWriteAccessEnabled()
        {
            UploaderTab.PkgChecker.CheckPackageCmd.ExecuteIfItCan();
            //PreviousVerTab.GetVersionsCmd.ExecuteIfItCan();
        }
    }
}
