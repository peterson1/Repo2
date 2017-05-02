using System;
using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public class SubjectAlterations : List<SubjectValueMod>
    {
        public SubjectAlterations(int actorId, uint subjectId)
        {
            ActorID   = actorId;
            SubjectID = subjectId;
        }


        public void Add (string fieldName, object newValue)
        {
            Add(new SubjectValueMod
            {
                Timestamp = DateTime.Now,
                ActorID   = this.ActorID,
                SubjectID = this.SubjectID,
                FieldName = fieldName,
                NewValue  = newValue
            });
        }


        public int   ActorID    { get; }
        public uint  SubjectID  { get; }
    }
}
