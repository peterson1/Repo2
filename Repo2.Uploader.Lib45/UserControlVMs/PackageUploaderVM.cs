using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.SDK.WPF45.InputCommands;

namespace Repo2.Uploader.Lib45.UserControlVMs
{
    [ImplementPropertyChanged]
    public class PackageUploaderVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public event EventHandler<Reply>         UploadFinished  = delegate { };

        private IPackageUploader _pkgUploadr;


        public PackageUploaderVM(IPackageUploader packageUploader)
        {
            _pkgUploadr = packageUploader;

            _pkgUploadr.StatusChanged += (s, statusText)
                => UploaderStatus = statusText;

            PropertyChanged += (s, e) 
                => CommandManager.InvalidateRequerySuggested();

            CreateCommands();
        }


        public R2Package    Package          { get; private set; }
        public string       UploaderStatus   { get; private set; }
        public IR2Command   StartUploadCmd   { get; private set; }
        public IR2Command   StopUploadCmd    { get; private set; }
        public double       MaxPartSizeMB    { get; set; } = 0.5;
        public string       RevisionLog      { get; set; }


        internal void EnableUpload  (R2Package pkg) => Package = pkg;
        internal void DisableUpload ()              => Package = null;


        private async Task StartUpload()
        {
            Clipboard.SetText(RevisionLog);

            _pkgUploadr.MaxPartSizeMB = this.MaxPartSizeMB;
            var reply = await _pkgUploadr.StartUpload(Package, RevisionLog);

            UploadFinished?.Invoke(this, reply);

            //Alerter.Show(reply, "Package Upload");

            //if (reply.IsSuccessful)
            //    CheckUploadabilityCmd.ExecuteIfItCan();
        }


        private bool CanUpload()
        {
            if (Package == null) return false;
            if (MaxPartSizeMB <= 0) return false;
            if (RevisionLog.IsBlank()) return false;
            return true;
        }


        private void StopUpload()
        {
            _pkgUploadr.StopUpload();
            StartUploadCmd.ConcludeExecute();
        }


        private void CreateCommands()
        {
            StartUploadCmd = R2Command.Async(StartUpload,
                         x => CanUpload(), "Upload Package");

            StopUploadCmd = R2Command.Relay(StopUpload,
                        x => _pkgUploadr.IsUploading, "stop uploading");
        }
    }
}
