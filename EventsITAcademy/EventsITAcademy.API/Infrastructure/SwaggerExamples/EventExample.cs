using EventsITAcademy.Application.Events.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Infrastructure.SwaggerExamples
{
    public class EventExample : IExamplesProvider<EventResponseModel>
    {
        public EventResponseModel GetExamples()
        {
            return new EventResponseModel
            {
                Id = 1,
                Title = "Formula 1 აზერბაიჯანის გრან პრი",
                StartDate = DateTime.Now.AddDays(2),
                FinishDate = DateTime.Now.AddDays(2).AddHours(2),
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsArchived = false,
                Description = "❖ 28 აპრილი - პირველი პრაქტიკა (13:30-14:30); კვალიფიკაცია (17:00-18:00)\r\n\r\n❖ 29 აპრილი - მეორე პრაქტიკა (13:30-14:30); სპრინტ რბოლა (17:30)\r\n\r\n❖ 30 აპრილი - აზერბაიჯანის გრან პრი (15:00)",
                OwnerId = new Guid().ToString(),
                ModificationPeriod = 1,
                NumberOfTickets = 500,
                ReservationPeriod = 20
            };
        }
    }
}
