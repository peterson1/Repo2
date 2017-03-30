using System;
using System.Collections.Generic;
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
        Task<bool>    DisableWriteAccess      ();
        bool          IsEnablingWriteAccess   { get; }


        Task<List<TModel>> Paged<TModel, TDto>(
            int pageNumber,
            int itemsPerPage,
            CancellationToken cancelTkn)
                where TModel : class
                where TDto : TModel, IRestExportView, new();


        Task<List<TModel>> PagedSerial<TModel, TDto>(
            int itemsPerPage,
            int totalItemsCount,
            CancellationToken cancelTkn)
                where TModel : class
                where TDto : TModel, IRestExportView, new();


        Task<List<TModel>> PagedParallel<TModel, TDto>(
            int itemsPerPage,
            int totalItemsCount,
            int requestGapMS,
            CancellationToken cancelTkn)
                where TModel : class
                where TDto : IRestExportView, TModel, new();



        Task<List<T>>  List  <T>(CancellationToken cancelTkn, params object[] args)
            where T : IRestExportView, new();


        Task<List<TModel>>  List  <TModel, TDto>(CancellationToken cancelTkn, params object[] args)
            where TModel : class
            where TDto   : TModel, IRestExportView, new();


        Task<List<object>> ListRevisions (string nodeTitle, CancellationToken cancelTkn);


        Task<byte[]>     GetBytes   <T>(CancellationToken cancelTkn, params object[] args) 
            where T : IRestExportView, IBase64Content, new();


        Task<NodeReply>  PostNode   <T>(T node, CancellationToken cancelTkn) 
            where T : ID8Node;


        Task<NodeReply>  PatchNode  <T>(T node, CancellationToken cancelTkn, string revisionLog = null) 
            where T : D8NodeBase;


        Task<RestReply>  DeleteNode (int nodeID, CancellationToken cancelTkn);
    }
}
