using System;
using System.Collections.Generic;
using System.Linq;

namespace Repo2.Core.ns11.DataStructures
{
    public class SubjectAlterations : List<SubjectValueMod>
    {
        public SubjectAlterations(int actorId, uint subjectId)
        {
            ActorID   = actorId;
            SubjectID = subjectId;
        }


        public void Add (string fieldName, object newValue, DateTime? timestamp = null)
        {
            Add(new SubjectValueMod
            {
                Timestamp = timestamp ?? DateTime.Now,
                ActorID   = this.ActorID,
                SubjectID = this.SubjectID,
                FieldName = fieldName,
                NewValue  = newValue
            });
        }


        public int   ActorID    { get; }
        public uint  SubjectID  { get; }


        public SubjectAlterations GetChanges(IEnumerable<SubjectValueMod> savedMods)
        {
            var changes = new SubjectAlterations(this.ActorID, this.SubjectID);

            foreach (var newRow in this)
            {
                var oldRow = savedMods.LastOrDefault(x => x.FieldName == newRow.FieldName);
                if (oldRow == null)
                {
                    changes.Add(newRow);
                    continue;
                }

                if (oldRow.NewValue == null 
                 && newRow.NewValue == null) continue;


                if (oldRow.NewValue == null
                 && newRow.NewValue != null)
                {
                    changes.Add(newRow);
                    continue;
                }

                if (oldRow.NewValue != null
                 && newRow.NewValue == null)
                {
                    changes.Add(newRow);
                    continue;
                }

                if (oldRow.NewValue.ToString() != newRow.NewValue.ToString())
                    changes.Add(newRow);
            }
            return changes;
        }

        //public SubjectAlterations ApplyTo(List<SubjectValueMod> oldValues)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
