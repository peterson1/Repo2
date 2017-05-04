using System;

namespace Repo2.Core.ns11.DataStructures
{
    public class SubjectValueMod
    {
        public ulong     Id          { get; set; }
        public DateTime  Timestamp   { get; set; }
        public int       ActorID     { get; set; }
        public uint      SubjectID   { get; set; }
        public string    FieldName   { get; set; }
        public object    NewValue    { get; set; }
    }
}
