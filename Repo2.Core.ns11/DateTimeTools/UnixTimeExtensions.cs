using System;

namespace Repo2.Core.ns11.DateTimeTools
{
    public static class UnixTimeExtensions
    {
        static DateTime EpochDate => new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

        public static DateTime ToDateFromUnixSeconds(this double unixTimeSeconds)
            => EpochDate.AddSeconds(unixTimeSeconds).ToLocalTime();
    }
}
