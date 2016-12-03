using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DataStructures;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.Exceptions;
using Repo2.Core.ns11.Extensions.StringExtensions;
using Repo2.Core.ns11.FileSystems;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8ErrorTicketManager1 : IErrorTicketManager
    {
        private IR2RestClient      _client;
        private IFileSystemAccesor _fs;

        public D8ErrorTicketManager1(IR2RestClient r2RestClient, IFileSystemAccesor fileSystemAccesor)
        {
            _client = r2RestClient;
            _fs     = fileSystemAccesor;
        }


        public Task<List<R2ErrorTicket>> List(ErrorState ticketStatus, CancellationToken cancelTkn)
            => _client.List<R2ErrorTicket, ErrorsByStatus1>(cancelTkn, (int)ticketStatus);


        public async Task<bool> Post(R2ErrorTicket r2ErrorTicket)
        {
            NodeReply rep = null;
            try
            {
                rep = await _client.PostNode(r2ErrorTicket, new CancellationToken());
            }
            catch (Exception ex)
            {
                LogErrorInPosting(r2ErrorTicket, ex);
                return false;
            }

            if (rep == null)
            {
                LogFailedReply(null, r2ErrorTicket);
                return false;
            }
            if (rep.Failed)
            {
                LogFailedReply(rep , r2ErrorTicket);
                return false;
            }
            return true;
        }


        private void LogFailedReply(NodeReply rep, R2ErrorTicket tkt)
        {
            var content = rep == null ? "NULL" : "FAILED";
            content = $"Received {content} reply after posting error ticket.";

            if (rep != null)
            {
                content += $"{L.F}{rep.ErrorsText}";
                if (rep.HasWarnings) content += $"{L.F}{rep.WarningsText}";
            }

            LogToFile("FailedReply", tkt, content);
        }


        private void LogErrorInPosting(R2ErrorTicket tkt, Exception ex)
            => LogToFile("RaisedException", tkt, 
                $"Exception raised while posting error ticket.{L.F}{ex.Info(true, true)}");


        private void LogToFile(string fileNameSuffix, R2ErrorTicket tkt, string moreContent)
        {
            var fName   = $"PostingError_{fileNameSuffix}";
            var content = $"{L.F}{tkt.ToString()}{L.F}{moreContent}";
            _fs.WriteRepo2LogFile(fName, content);
        }
    }
}
