// Copyright (C) TBC Bank. All Rights Reserved.

using EventsITAcademy.Application.Events.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Infrastructure.SwaggerExamples
{
    public class EventExamples : IMultipleExamplesProvider<EventResponseModel>
    {
        public IEnumerable<SwaggerExample<EventResponseModel>> GetExamples()
        {
            yield return SwaggerExample.Create("example 1", new EventResponseModel
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
            });

            yield return SwaggerExample.Create("example 2", new EventResponseModel
            {
                Id = 2,
                Title = "Example Event",
                StartDate = DateTime.Now.AddDays(5),
                FinishDate = DateTime.Now.AddDays(5).AddHours(3),
                CreatedAt = DateTime.Now,
                IsActive = true,
                IsArchived = false,
                Description = "Event Description...",
                OwnerId = new Guid().ToString(),
                ModificationPeriod = 1,
                NumberOfTickets = 200,
                ReservationPeriod = 10
            });
        }
    }
}
