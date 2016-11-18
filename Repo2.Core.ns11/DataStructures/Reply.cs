using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public class Reply
    {
        public List<string>   Errors    { get; set; } = new List<string>();
        public List<string>   Warnings  { get; set; } = new List<string>();

        public bool IsSuccessful => Errors.Count == 0;
        public bool HasWarnings  => Warnings.Count != 0;
        public bool Failed       => !IsSuccessful;
    }


    public class Reply<T> : Reply
    {
        public T  Result  { get; set; }
    }
}
