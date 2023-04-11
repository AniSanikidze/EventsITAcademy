using EventsITAcademy.Application.Tickets.Responses;

namespace EventsITAcademy.Application.Tickets
{
    public interface ITicketService
    {
        Task<List<TicketResponseModel>> GetAllReservedAsync(CancellationToken cancellationToken);
        Task<TicketResponseModel> Reserve(CancellationToken cancellationToken, int eventId, String userId);
        Task<TicketResponseModel> Buy(CancellationToken cancellationToken, int eventId, String userId);
        Task<bool> RemoveReservationAsync(CancellationToken cancellationToken, string userId, int eventId);
    }
}
