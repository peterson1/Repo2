using System;
using System.Text;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.Drupal8.Attributes;
using Repo2.Core.ns11.Exceptions;

namespace Repo2.Core.ns11.DomainModels
{
    public enum ErrorState
    {
        Unknown = 0,
        NeedsReview,
        NowFixing,
        Fixed,
        Closed_Duplicate,
        Closed_EdgeCase,
    }

    public class R2ErrorTicket : D8NodeBase
    {
        public override string D8TypeName => "error_ticket";

        [ContentTitle]         public string   Title         { get; set; }
        [_("description")]     public string   Description   { get; set; }
        [_("windows_account")] public string   WindowsAcct   { get; set; }
        [_("computer_name")]   public string   ComputerName  { get; set; }

        public ErrorState TicketStatus { get; set; }


        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"        title :  {Title}");
            sb.AppendLine($"  description :  {Description}");
            sb.AppendLine($" Windows acct :  {WindowsAcct}");
            sb.AppendLine($"      PC name :  {ComputerName}");
            return sb.ToString();
        }


        public static R2ErrorTicket From<T>(T exceptionObj)
            => exceptionObj is Exception ? CastError(exceptionObj)
                                         : CastNonError(exceptionObj);


        private static R2ErrorTicket CastError<T>(T exceptionObj)
            => new R2ErrorTicket
            {
                Title       = (exceptionObj as Exception).Message,
                Description = GetDescription(exceptionObj),
            };


        private static R2ErrorTicket CastNonError<T>(T exceptionObj)
            => new R2ErrorTicket
            {
                Title = $"‹{typeof(T).Name}› (non-Exception type)",
                Description = GetDescription(exceptionObj),
            };


        private static string GetDescription<T>(T exceptionObj)
        {
            if (exceptionObj == null)
                return $"NULL exception object received by global handler.";

            return (exceptionObj as Exception)?.Info(true, true) 
                ?? $"Non-Exception object thrown: ‹{typeof(T).Name}›";
        }
    }
}
