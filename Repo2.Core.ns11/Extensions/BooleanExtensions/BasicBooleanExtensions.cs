namespace Repo2.Core.ns11.Extensions.BooleanExtensions
{
    public static class BasicBooleanExtensions
    {
        public static string ToYesNo(this bool boolean)
            => boolean ? "Yes" : "No";
    }
}
