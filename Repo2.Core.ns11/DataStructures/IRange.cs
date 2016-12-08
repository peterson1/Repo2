namespace Repo2.Core.ns11.DataStructures
{
    // from: http://stackoverflow.com/a/4781727/3973863
    public interface IRange<T>
    {
        T    Start    { get; }
        T    End      { get; }
        bool Includes (T value);
        bool Includes (IRange<T> range);
    }
}
