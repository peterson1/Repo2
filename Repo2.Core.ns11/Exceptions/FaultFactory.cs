using System;
using System.IO;
using Repo2.Core.ns11.Extensions.StringExtensions;
using System.Runtime.CompilerServices;
using Repo2.Core.ns11.DataStructures;

namespace Repo2.Core.ns11.Exceptions
{
    public class Fault
    {
        public static MissingMemberException NoMember(string memberName)
            => new MissingMemberException(
                $"Missing member: “{memberName}”");


        public static MissingMemberException NoItems(string listDescription)
            => new MissingMemberException(
                $"No items found in list of “{listDescription}”");


        public static ArgumentException NoMatch<T>(string key, object value, string operatr = "==")
            => new ArgumentException(
                $"No ‹{typeof(T).Name}› found where [{key}] {operatr} “{value}”.");


        public static InvalidOperationException MultiMatch<T>(string key, object value, string operatr = "==")
            => new InvalidOperationException(
                $"Multiple ‹{typeof(T).Name}› found where [{key}] {operatr} “{value}”.");


        public static InvalidOperationException NonSolo(string listDescription, int actualItemCount)
            => new InvalidOperationException(
                $"Expected list of {listDescription} to contain a SINGLE item, but found [{actualItemCount}].");


        public static InvalidOperationException BadCall(string requiredMethod, [CallerMemberName] string attemptedMethod = "")
            => new InvalidOperationException(
                $"Please call [{requiredMethod}()] before calling [{attemptedMethod}()].");


        public static ArgumentException BadArg(string parameterName, string reasonPhrase, [CallerMemberName] string caller = "")
            => new ArgumentException(
                $"Method “{caller}()” expects parameter “{parameterName}” to {reasonPhrase}.");


        public static FileNotFoundException Missing(string fileDescription, string filePath)
            => new FileNotFoundException(
                $"“{fileDescription}” not found in:{L.f}{filePath}");


        public static InvalidCastException BadCast<T>(object obj)
            => new InvalidCastException(
                $"Failed to cast ‹{obj?.GetType().Name}› “{obj.ToString()}” to ‹{typeof(T).Name}›");


        public static NullReferenceException NullRef<T>(string memberName)
            => new NullReferenceException(
                $"‹{typeof(T).Name}› “{memberName}” is NULL.");


        public static InvalidDataException BadData<T>(T invalidObj, string validationError)
            => new InvalidDataException(
                $"‹{typeof(T).Name}› contains invalid data.{L.f}{validationError}");


        public static ArgumentException BlankText(string textDescription)
            => new ArgumentException(
                $"“{textDescription}” should not be BLANK.");


        public static DataMisalignedException HashMismatch(string hashSrc1, string hashSrc2)
            => new DataMisalignedException(
                $"Hash of {hashSrc1} did not match hash of {hashSrc2}.");


        public static UnauthorizedAccessException CantMove(string originalPath, string targetPath)
            => new UnauthorizedAccessException(
                $"Failed to move file to target location.{L.f}source :  {originalPath}{L.f}target :  {targetPath}");


        public static InvalidOperationException Failed (string operationDesc)
            => new InvalidOperationException(
                $"Operation “{operationDesc}” failed.");

        public static InvalidOperationException Failed(string operationDesc, Reply reply)
            => new InvalidOperationException(
                $"Operation “{operationDesc}” failed.{L.F}{reply.ErrorsText}");


        public static NotSupportedException Unsupported(object unsupportedValue)
            => new NotSupportedException(
                $"Unsupported value: “{unsupportedValue}”.");
    }
}
