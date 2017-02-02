using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.Authentication;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.Drupal8;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.RestClients
{
    public interface IR2RestClient
    {
        event EventHandler<string> OnRetry;

        void          SetCredentials          (R2Credentials credentials, bool addCertToWhiteList = true);
        Task<bool>    EnableWriteAccess       (R2Credentials credentials, bool addCertToWhiteList = true);
        void          StopEnablingWriteAccess ();
        Task<string>  DisableWriteAccess      ();
        bool          IsEnablingWriteAccess   { get; }


        Task<List<T>>  List  <T>(CancellationToken cancelTkn, params object[] args)
            where T : IRestExportView, new();

        Task<List<TModel>>  List  <TModel, TDto>(CancellationToken cancelTkn, params object[] args)
            where TModel : class
            where TDto   : TModel, IRestExportView, new();

        Task<List<object>> ListRevisions (string nodeTitle, CancellationToken cancelTkn);

        Task<byte[]>     GetBytes   <T>(CancellationToken cancelTkn, params object[] args) 
            where T : IRestExportView, IBase64Content, new();

        Task<NodeReply>  PostNode   <T>(T node, CancellationToken cancelTkn) 
            where T : D8NodeBase;

        Task<NodeReply>  PatchNode  <T>(T node, CancellationToken cancelTkn, string revisionLog = null) 
            where T : D8NodeBase;

        Task<RestReply>  DeleteNode (int nodeID, CancellationToken cancelTkn);
    }
}
