using Repo2.Core.ns11.ChangeNotification;
using Repo2.Core.ns11.DataStructures;
using System;
using System.Collections.Generic;

namespace Repo2.Core.ns11.Databases
{
    public interface ISubjectAlterationsDB : IStatusChanger
    {
        event EventHandler<Tuple<SubjectAlterations, uint>> SubjectCreated;
        event EventHandler<List<SubjectValueMod>>           SubjectUpdated;

        uint                     CreateNewSubject  (SubjectAlterations mods);
        //List<SubjectValueMod>    GetAllMods        (uint subjectId);
        //uint                     GetNextSubjectId  ();
    }
}
