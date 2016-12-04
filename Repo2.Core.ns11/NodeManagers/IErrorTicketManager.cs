using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Repo2.Core.ns11.DomainModels;

namespace Repo2.Core.ns11.NodeManagers
{
    public interface IErrorTicketManager
    {
        Task<List<R2ErrorTicket>> List(ErrorState ticketStatus, CancellationToken cancelTkn);
        void Post(R2ErrorTicket r2ErrorTicket);
    }
}
