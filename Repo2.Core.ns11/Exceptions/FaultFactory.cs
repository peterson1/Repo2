using System;
using System.IO;
using Repo2.Core.ns11.Extensions.StringExtensions;

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


        public static OverflowException NonSolo(string listDescription, int actualItemCount)
            => new OverflowException(
                $"Expected list of “{listDescription}” to contain a [single] item, but found [{actualItemCount}].");


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


        public static InvalidDataException BlankText(string textDescription)
            => new InvalidDataException(
                $"“{textDescription}” should not be BLANK.");


        public static DataMisalignedException HashMismatch(string hashSrc1, string hashSrc2)
            => new DataMisalignedException(
                $"Hash of {hashSrc1} did not match hash of {hashSrc2}.");
    }
}
