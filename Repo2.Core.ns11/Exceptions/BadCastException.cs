using Repo2.Core.ns11.Extensions.StringExtensions;
using System;

namespace Repo2.Core.ns11.Exceptions
{
    public class BadCastException : Exception
    {
        public BadCastException(object uncastableObj, Type sourceType, Type targetType, Exception innerException)
            : base(FormatMessage(uncastableObj, sourceType, targetType, innerException), innerException)
        {
            SourceType     = sourceType;
            TargetType     = targetType;
            SourceTypeName = sourceType?.Name;
            TargetTypeName = targetType?.Name;
            Uncastable     = uncastableObj;
        }


        public Type     SourceType       { get; }
        public Type     TargetType       { get; }
        public string   SourceTypeName   { get; }
        public string   TargetTypeName   { get; }
        public object   Uncastable       { get; }

        public string Title => $"Failed to cast ‹{SourceTypeName}› to ‹{TargetTypeName}›.";


        private static string FormatMessage(object uncastableObj, Type sourceType, Type targetType, Exception innerException)
        {
            var title  = $"Failed to cast ‹{sourceType.Name}› to ‹{targetType.Name}›.";
            var inrErr = innerException.Info(true, true);
            return $"{title}{L.F}{inrErr}{L.F}{uncastableObj}";
        }


        public static BadCastException Create<TTarget>(
            object uncastableObj, Exception innerException)
                => new BadCastException(uncastableObj, 
                                        uncastableObj.GetType(), 
                                        typeof(TTarget), 
                                        innerException);
    }
}
