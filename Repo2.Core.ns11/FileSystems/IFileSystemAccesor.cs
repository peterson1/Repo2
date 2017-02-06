using System.Collections.Generic;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.FileSystems
{
    public interface IFileSystemAccesor
    {
        string TempDir { get; }

        Task<string> IsolateFile       (R2Package localPkg);
        Task<bool>   Delete            (params string[] filePaths);
        Task<bool>   Delete            (IEnumerable<string> filePaths);
        string       GetSHA1           (string filePath);

        string       WriteTempFile        (byte[] byts);
        bool         WriteRepo2LogFile    (string fileNameSuffix, string content, bool raiseError = false);
        void         WriteTextFile        (string text, string filePath);
        void         WriteJsonFile        (object objectToSerialize, string filePath);

        T            ReadDesktopJsonFile    <T>(string filename);
        string       WriteDesktopFile       (string filename, string contents);
        string       WriteDesktopJsonFile   <T>(string filename, T objectToSerialize);

        T            ReadJsonFileBesideExe  <T>(string filename);
        string       WriteFileBesideExe     (string filename, string contents);
        string       WriteJsonFileBesideExe <T>(string filename, T objectToSerialize);

        bool DesktopFileFound(string filename);

        string       GetTempFilePath         (string filename = null);
        string       GetDesktopFilePath      (string filename);
        string       GetBesideExeFilePath    (string filename);
        string       GetAppDataFilePath      (string subFoldername, string filename, string parentDir = "Repo2");

        string       CurrentExeFile      { get; }
        string       CurrentExeVersion   { get; }

        string       ReadBase64    (string filePath);
        bool         Found         (string filePath);
        R2Package    ToR2Package   (string filePath);
        Task<bool>   Move          (string originalPath, string targetPath);

        T            DecryptJsonFile   <T>(string filePath, string decryptKey);
        void         EncryptJsonToFile <T>(string filePath, T obj, string encryptKey);

        //string       Chain           (params string[] foldersAndFilename);
        //void        AppendTo        (string filePath, string text);
        void        CreateDir       (string foldrPath);
    }
}
