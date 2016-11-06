using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.SDK.WPF45.Authentication
{
    public class Certificator
    {
        //const string IPIFY_CERT = "68BD712DFC7529ED73D2E5E3F1A4EB5DFBA50164";
        //const string NFSHOST_CERT = "341E845315CF8CFF77428ABC4A0394E31133DB7C";
        //const string FATS_CERT = "CE2D7840286F8571E3F58203E4976F1DE6D0DDBA";


        private static List<string> _whiteList = new List<string>
        {
            //NFSHOST_CERT,
            //FATS_CERT
        };


        public static void AllowFrom(params string[] serverThumbPrints)
        {
            _whiteList.AddRange(serverThumbPrints);
            SetGlobalValidationCallback();
        }


        private static void SetGlobalValidationCallback()
        {
            ServicePointManager.ServerCertificateValidationCallback -= ValidationCallback;
            ServicePointManager.ServerCertificateValidationCallback += ValidationCallback;
        }


        private static bool ValidationCallback(object sender, 
                                               X509Certificate certificate, 
                                               X509Chain chain, 
                                               SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;

            var x509Cert = certificate as X509Certificate2;
            if (x509Cert == null) return false;
            if (_whiteList.Count == 0) return false;

            var listed = _whiteList.Contains(x509Cert.Thumbprint);

            if (!listed)
            {
                var msg = $"Unlisted: {x509Cert.Thumbprint}" + L.f 
                    + "DnsName :  " + x509Cert.GetNameInfo(X509NameType.DnsName, true) + L.f
                    + "DnsFrom :  " + x509Cert.GetNameInfo(X509NameType.DnsFromAlternativeName, true) + L.f
                    + "EmailNa :  " + x509Cert.GetNameInfo(X509NameType.EmailName, true) + L.f
                    + "SimpleN :  " + x509Cert.GetNameInfo(X509NameType.SimpleName, true) + L.f
                    + "UpnName :  " + x509Cert.GetNameInfo(X509NameType.UpnName, true) + L.f
                    + "UrlName :  " + x509Cert.GetNameInfo(X509NameType.UrlName, true) + L.f
                    + "Issuer  :  " + x509Cert.Issuer + L.f
                    + "Subject :  " + x509Cert.Subject;

                //MessageBox.Show(msg);
                //Console.WriteLine(msg);
                throw new System.UnauthorizedAccessException(msg);
            }


            return listed;
        }
    }
}
