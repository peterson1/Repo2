using System;

namespace Repo2.Core.ns11.Authentication
{
    public class R2Credentials : IR2Credentials
    {
        public string  Username             { get; set; }
        public string  Password             { get; set; }
        public string  BaseURL              { get; set; }
        public string  CertificateThumb     { get; set; }
        public int     CheckIntervalSeconds { get; set; }

        public TimeSpan CheckInterval 
            => TimeSpan.FromSeconds(CheckIntervalSeconds);
    }
}
