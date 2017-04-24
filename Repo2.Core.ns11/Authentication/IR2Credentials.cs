using System;

namespace Repo2.Core.ns11.Authentication
{
    public interface IR2Credentials
    {
        string   Username             { get; }
        string   Password             { get; }
        string   BaseURL              { get; }
        string   CertificateThumb     { get; }
        int      CheckIntervalSeconds { get; }

        //TimeSpan CheckInterval        { get; }
    }
}
