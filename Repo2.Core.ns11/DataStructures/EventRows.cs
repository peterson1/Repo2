using System;
using System.Collections.Generic;

namespace Repo2.Core.ns11.DataStructures
{
    public class EventRows : List<EventRow>
    {
        public EventRows(int userId)
        {
            UserID = userId;
        }


        public void Add (string fieldName, object fieldValue)
        {
            Add(new EventRow
            {
                Timestamp  = DateTime.Now,
                UserID     = this.UserID,
                FieldName  = fieldName,
                FieldValue = fieldValue
            });
        }


        public int UserID { get; }
    }
}
