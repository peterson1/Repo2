using System;

namespace Repo2.Core.ns11.Exceptions
{
    public class Fault
    {
        public static MissingMemberException Missing(string memberName)
            => new MissingMemberException(
                $"Missing member: “{memberName}”");


        public static NullReferenceException NullRef<T>(string memberName)
            => new NullReferenceException(
                $"‹{typeof(T).Name}› “{memberName}” is NULL.");


        public static DataMisalignedException HashMismatch(string hashSrc1, string hashSrc2)
            => new DataMisalignedException(
                $"Hash of {hashSrc1} did not match hash of {hashSrc2}.");
    }
}
