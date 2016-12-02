using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;
using Repo2.Core.ns11.RestClients;
using Repo2.Core.ns11.RestExportViews;

namespace Repo2.Core.ns11.NodeManagers
{
    public class D8ErrorTicketManager1 : IErrorTicketManager
    {
        private IR2RestClient _client;

        public D8ErrorTicketManager1(IR2RestClient r2RestClient)
        {
            _client = r2RestClient;
        }


        public Task<List<R2ErrorTicket>> List(ErrorState ticketStatus, CancellationToken cancelTkn)
            => _client.List<R2ErrorTicket, ErrorsByStatus1>(cancelTkn, (int)ticketStatus);
        //{
        //    await Task.Delay(1);
        //    return new List<R2ErrorTicket>();
        //}
    }
}
