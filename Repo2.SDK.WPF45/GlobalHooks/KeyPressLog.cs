using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Repo2.SDK.WPF45.GlobalHooks
{
    public class KeyPressLog
    {
        public List<LogEntry>  Entries  { get; set; }


        public class LogEntry
        {
            public DateTime  Timestamp    { get; set; }
            public Key       KeyPressed   { get; set; }
            public string    KeyText      { get; set; }
            public string    WindowTitle  { get; set; }
        }


        public void Add(Key key, string windowTitle = null)
        {
            if (Entries == null)
                Entries = new List<LogEntry>();

            Entries.Add(new LogEntry
            {
                Timestamp   = DateTime.Now,
                KeyPressed  = key,
                KeyText     = key.ToString(),
                WindowTitle = windowTitle,
            });
        }
    }
}
