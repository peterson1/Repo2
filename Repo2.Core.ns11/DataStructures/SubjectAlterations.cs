using System;
using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public class SubjectAlterations : List<SubjectValueMod>
    {
        public SubjectAlterations(int userId, int subjectId)
        {
            UserID    = userId;
            SubjectID = subjectId;
        }


        public void Add (string fieldName, object newValue)
        {
            Add(new SubjectValueMod
            {
                Timestamp  = DateTime.Now,
                ActorID     = this.UserID,
                SubjectID  = this.SubjectID,
                FieldName  = fieldName,
                NewValue   = newValue
            });
        }


        public int  UserID     { get; }
        public int  SubjectID  { get; }
    }
}
