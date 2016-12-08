using System;
using Repo2.Core.ns11.DateTimeTools;

namespace Repo2.SDK.WPF45.UserInputWindows
{
    public class DateRangeDialogue
    {
        private DialogueWindow1 _win;

        public DateRangeDialogue()
        {
            StartDate = Today.FirstDayOfMonth();
            EndDate   = DateTime.Now;
        }

        public DateTime   StartDate    { get; set; }
        public DateTime   EndDate      { get; set; }
        public string     Caption      { get; set; } = "Please choose start and end dates.";
        public string     Title        { get; set; } = "Select Period";
        public string     Subtitle     { get; set; } = "date range";
        public string     ButtonLabel  { get; set; } = "Okay";


        public DateRange ShowDialog()
        {
            _win = new DialogueWindow1();
            _win.SetDates(StartDate, EndDate);
            _win.SetLabels(Caption, Title, Subtitle, ButtonLabel);


            var result = _win.ShowDialog();
            if (!(result ?? false)) return null;

            var selctdStart = _win.StartDate ?? DateTime.Now;
            var selctdEnd   = _win.EndDate   ?? DateTime.Now;

            return new DateRange(selctdStart, selctdEnd);
        }
    }
}
