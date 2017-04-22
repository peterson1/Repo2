using System;

namespace Repo2.Core.ns11.DataStructures
{
    public class EventRow
    {
        public DateTime  Timestamp   { get; set; }
        public int       UserID      { get; set; }
        public string    FieldName   { get; set; }
        public object    FieldValue  { get; set; }
    }
}
