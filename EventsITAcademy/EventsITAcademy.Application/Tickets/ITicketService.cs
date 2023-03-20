using EventsITAcademy.Application.Tickets.Requests;
using EventsITAcademy.Application.Tickets.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsITAcademy.Application.Tickets
{
    public interface ITicketService
    {
        Task<List<TicketResponseModel>> GetAllReservedAsync(CancellationToken cancellationToken);
        Task<TicketResponseModel> Reserve(CancellationToken cancellationToken, TicketRequestModel ticketRequest, String userId);
        Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, string userId, int eventId);
    }
}
