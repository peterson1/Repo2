namespace Repo2.SDK.WPF45.Configuration
{
    public class DownloaderConfigFile : LocalConfigFile
    {
        protected override string  SubFolder => @"Repo2\Downloader";
        protected override string  Prefix    => "Downloader_";

        public static DownloaderConfigFile Parse(string cfgKey)
            => new DownloaderConfigFile()
                .Parse<DownloaderConfigFile>(cfgKey);
    }
}
