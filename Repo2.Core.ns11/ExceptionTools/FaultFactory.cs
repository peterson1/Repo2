using System;

namespace Repo2.Core.ns11.ExceptionTools
{
    public class Fault
    {
        public static MissingMemberException Missing(string memberName)
            => new MissingMemberException(
                $"Missing member: “{memberName}”");


        public static NullReferenceException NullRef<T>(string memberName)
            => new NullReferenceException(
                $"‹{typeof(T).Name}› “{memberName}” is NULL.");
    }
}
