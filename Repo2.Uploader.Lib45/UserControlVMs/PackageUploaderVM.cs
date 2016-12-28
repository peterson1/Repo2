using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.InputCommands;
using Repo2.Core.ns11.PackageUploaders;
using Repo2.Core.ns11.RestClients;
using Repo2.SDK.WPF45.Exceptions;
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


        internal void EnableUpload  (R2Package pkg) => Package = pkg;
        internal void DisableUpload ()              => Package = null;


        private async Task StartUpload()
        {
            _pkgUploadr.MaxPartSizeMB = this.MaxPartSizeMB;
            var reply = await _pkgUploadr.StartUpload(Package);

            UploadFinished?.Invoke(this, reply);

            //Alerter.Show(reply, "Package Upload");

            //if (reply.IsSuccessful)
            //    CheckUploadabilityCmd.ExecuteIfItCan();
        }


        private void StopUpload()
        {
            _pkgUploadr.StopUpload();
            StartUploadCmd.ConcludeExecute();
        }


        private void CreateCommands()
        {
            StartUploadCmd = R2Command.Async(StartUpload,
                         x => (Package != null) && (MaxPartSizeMB > 0), "Upload Package");

            StopUploadCmd = R2Command.Relay(StopUpload,
                        x => _pkgUploadr.IsUploading, "stop uploading");
        }
    }
}
