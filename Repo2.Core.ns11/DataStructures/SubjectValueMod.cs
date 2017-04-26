using System;

namespace Repo2.Core.ns11.DataStructures
{
    public class SubjectValueMod
    {
        public DateTime  Timestamp   { get; set; }
        public int       ActorID     { get; set; }
        public int       SubjectID   { get; set; }
        public string    FieldName   { get; set; }
        public object    NewValue    { get; set; }
    }
}
