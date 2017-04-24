using System;
using System.Collections.Generic;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;

namespace Repo2.Core.ns11.DataStructures
{
    public class Reply
    {
        public List<string>   Errors    { get; set; } = new List<string>();
        public List<string>   Warnings  { get; set; } = new List<string>();

        public bool    IsSuccessful  => Errors.Count == 0;
        public bool    HasWarnings   => Warnings.Count != 0;
        public bool    Failed        => !IsSuccessful;
        public string  ErrorsText    => Errors.Join(L.f);
        public string  WarningsText  => Warnings.Join(L.f);

        public static Reply Success() => new Reply();

        public static Reply<T> Success<T>(T result) => new Reply<T>(result);

        public static Reply Error(string errorMessage)
        {
            var rep = new Reply();
            rep.Errors.Add(errorMessage);
            return rep;
        }


        public static Reply<T> Error<T>(string errorMessage)
        {
            var rep = new Reply<T>(default(T));
            rep.Errors.Add(errorMessage);
            return rep;
        }

        public static Reply Warning(string warningMessage)
        {
            var rep = new Reply();
            rep.Warnings.Add(warningMessage);
            return rep;
        }
    }


    public class Reply<T> : Reply
    {
        public Reply()
        {
        }

        public Reply(T result)
        {
            Result = result;
        }

        public T  Result  { get; set; }
    }
}
