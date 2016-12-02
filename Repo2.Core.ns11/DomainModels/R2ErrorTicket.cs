using Repo2.Core.ns11.Drupal8.Attributes;

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

    public class R2ErrorTicket
    {
        [ContentTitle]         public string   Title         { get; set; }
        [_("description")]     public string   Description   { get; set; }
        [_("windows_account")] public string   WindowsAcct   { get; set; }
        [_("computer_name")]   public string   ComputerName  { get; set; }
        public ErrorState TicketStatus { get; set; }
    }
}
