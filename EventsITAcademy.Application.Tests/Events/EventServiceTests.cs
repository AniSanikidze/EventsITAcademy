using System.Linq.Expressions;
using EventsITAcademy.Application.Events;
using EventsITAcademy.Application.Events.Repositories;
using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Images;
using EventsITAcademy.Application.Users.Repositories;
using EventsITAcademy.Application.Users;
using EventsITAcademy.Domain.Events;
using Moq;
using EventsITAcademy.Application.CustomExceptions;
using EventsITAcademy.Domain.Users;

namespace EventsITAcademy.Application.Tests.Events
{
    public class EventServiceTests
    {
        private readonly EventRequestModel _requestModel;
        private readonly UpdateEventRequestModel _updateRequestModel;

        public EventServiceTests()
        {
            _requestModel = GetRequestModel();
            _updateRequestModel = GetUpdateRequestModel();
        }

        [Fact]
        public async Task CreateEvent_WhenDataIsCorrect_ShouldReturnEventId()
        {
            //arrange
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.CreateAsync(cancelationToken, It.IsAny<Event>())).ReturnsAsync(1);

            //act
            var newId = await eventService.CreateAsync(cancelationToken, _requestModel, new Guid().ToString()).ConfigureAwait(false);

            //assert
            Assert.Equal(1, newId);
        }

        [Fact]
        public async Task GetEvent_WhenExists_ShouldReturnEvent()
        {
            //arrange
            var eventId = 1;
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(x => x.GetAsync(cancelationToken, eventId)).ReturnsAsync(new Event
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
            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(true);

            //act
            var result = await eventService.GetAsync(cancelationToken, eventId).ConfigureAwait(false);

            //assert
            Assert.NotNull(result);
            Assert.Equal(eventId, result.Id);
            Assert.IsType<EventResponseModel>(result);

            mockEventRepo.Verify(x => x.GetAsync(cancelationToken, eventId), Times.Once);
        }

        [Fact]
        public async Task GetEvent_WhenDoesNotExist_ShouldThrowException()
        {
            //arrange
            var eventId = 13;
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(false);

            //act
            var task = async () => await eventService.GetAsync(cancelationToken, eventId).ConfigureAwait(false);

            //assert
            await Assert.ThrowsAnyAsync<ItemNotFoundException>(task).ConfigureAwait(false);
        }

        [Fact]
        public async Task GetEvents_ShouldReturnEvents()
        {
            //arrange
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.GetAllAsync(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(new List<Event>
            {
                new Event
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
                },
                new Event
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
                }
            });

            //act
            var result = await eventService.GetAllAsync(cancelationToken).ConfigureAwait(false);

            //assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<EventResponseModel>>(result);
        }

        [Fact]
        public async Task GetConfimredEvents_ShouldReturnEvents()
        {
            //arrange
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.GetAllAsync(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(new List<Event>
            {
                new Event
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
                },
                new Event
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
                }
            });

            //act
            var result = await eventService.GetAllAsync(cancelationToken).ConfigureAwait(false);

            //assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<List<EventResponseModel>>(result);

            //mockEventRepo.Verify(x => x.GetAllAsync(cancelationToken, x => x.IsActive == true && x.Status == Domain.EntityStatuses.Active), Times.Once);
        }

        [Fact]
        public async Task DeleteEvent_WhenEventIdIsCorrect_ShouldReturnEmptyTask()
        {
            //arrange
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.DeleteAsync(cancelationToken, It.IsAny<int>())).ReturnsAsync(5);
            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(true);

            //act
            var deletedId = await eventService.DeleteAsync(cancelationToken, 5).ConfigureAwait(false);

            //assert
            Assert.Equal(5, deletedId);
        }

        [Fact]
        public async Task DeleteEvent_WhenEventIdIsInCorrect_ShouldThrowException()
        {
            //arrange
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.DeleteAsync(cancelationToken, It.IsAny<int>())).ReturnsAsync(5);
            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(false);

            //act
            var task = async () => await eventService.DeleteAsync(cancelationToken, 7).ConfigureAwait(false);

            //assert
            await Assert.ThrowsAnyAsync<ItemNotFoundException>(task).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateEvent_WhenEventDoesNotExist_ShouldThrowItemNotFoundException()
        {
            //arrange
            var userId = new Guid().ToString();
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(false);

            //act
            var task = async () => await eventService.UpdateAsync(cancelationToken,_updateRequestModel,userId).ConfigureAwait(false);

            //assert
            await Assert.ThrowsAnyAsync<ItemNotFoundException>(task).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateEvent_WhenModificationDeadlinePassed_ShouldThrowEventModificationException()
        {
            //arrange
            var userId = new Guid().ToString();
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(true);
            mockEventRepo.Setup(expression: x => x.GetAsync(cancelationToken, It.IsAny<int>())).ReturnsAsync(new Event
            {
                ModificationPeriod = 1,
                CreatedAt = DateTime.Now.AddDays(-5),
            });

            //act
            var task = async () => await eventService.UpdateAsync(cancelationToken, _updateRequestModel, userId).ConfigureAwait(false);

            //assert
            await Assert.ThrowsAnyAsync<EventModificationException>(task).ConfigureAwait(false);
        }

        [Fact]
        public async Task UpdateEvent_WhenDataIsCorrect_ShouldReturnEventId()
        {
            //arrange
            var userId = new Guid().ToString();
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(true);
            mockEventRepo.Setup(expression: x => x.UpdateAsync(cancelationToken, It.IsAny<Event>())).ReturnsAsync(_updateRequestModel.Id);
            mockEventRepo.Setup(x => x.GetAsync(cancelationToken, It.IsAny<int>())).ReturnsAsync(new Event
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
            //act
            var eventId = await eventService.UpdateAsync(cancelationToken, _updateRequestModel, userId).ConfigureAwait(false);

            //assert

            Assert.Equal(eventId,_updateRequestModel.Id);
        }
        [Fact]
        public async Task ArchiveEvent_WhenDataIsCorrect_ShouldReturnEventId()
        {
            //arrange
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(true);
            mockEventRepo.Setup(expression: x => x.UpdateAsync(cancelationToken, It.IsAny<Event>())).ReturnsAsync(_updateRequestModel.Id);
            mockEventRepo.Setup(x => x.GetAsync(cancelationToken, It.IsAny<int>())).ReturnsAsync(new Event
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
            //act
            var eventId = await eventService.ArchiveEvent(cancelationToken, _updateRequestModel.Id).ConfigureAwait(false);

            //assert

            Assert.Equal(eventId, _updateRequestModel.Id);
        }
        [Fact]
        public async Task ArchiveEvent_WhenEventNotFound_ShouldThrowNotFoundException()
        {
            //arrange
            var mockEventRepo = new Mock<IEventRepository> { DefaultValue = DefaultValue.Empty };
            var mockImageService = new Mock<IImageService> { DefaultValue = DefaultValue.Empty };
            var eventService = new EventService(mockEventRepo.Object, mockImageService.Object);

            var cancelationToken = new CancellationToken();

            mockEventRepo.Setup(expression: x => x.Exists(cancelationToken, It.IsAny<Expression<Func<Event, bool>>>())).ReturnsAsync(false);
            mockEventRepo.Setup(expression: x => x.UpdateAsync(cancelationToken, It.IsAny<Event>())).ReturnsAsync(_updateRequestModel.Id);
            mockEventRepo.Setup(x => x.GetAsync(cancelationToken, It.IsAny<int>())).ReturnsAsync(new Event
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
            //act
            var task = async () => await eventService.ArchiveEvent(cancelationToken, _updateRequestModel.Id).ConfigureAwait(false);

            //assert
            await Assert.ThrowsAnyAsync<ItemNotFoundException>(task).ConfigureAwait(false);
        }

        private static EventRequestModel GetRequestModel()
        {
            return new EventRequestModel
            {
                Title = "Formula 1 აზერბაიჯანის გრან პრი",
                StartDate = DateTime.Now.AddDays(2),
                FinishDate = DateTime.Now.AddDays(2).AddHours(2),
                Description = "❖ 28 აპრილი - პირველი პრაქტიკა (13:30-14:30); კვალიფიკაცია (17:00-18:00)\r\n\r\n❖ 29 აპრილი - მეორე პრაქტიკა (13:30-14:30); სპრინტ რბოლა (17:30)\r\n\r\n❖ 30 აპრილი - აზერბაიჯანის გრან პრი (15:00)",
                NumberOfTickets = 500,
                ImageFile = null
            };
        }
        private static UpdateEventRequestModel GetUpdateRequestModel()
        {
            return new UpdateEventRequestModel
            {
                Id = 1,
                Title = "Formula 1 აზერბაიჯანის გრან პრი Updated",
                StartDate = DateTime.Now.AddDays(2),
                FinishDate = DateTime.Now.AddDays(2).AddHours(2),
                Description = "❖ 28 აპრილი - პირველი პრაქტიკა (13:30-14:30); კვალიფიკაცია (17:00-18:00)\r\n\r\n❖ 29 აპრილი - მეორე პრაქტიკა (13:30-14:30); სპრინტ რბოლა (17:30)\r\n\r\n❖ 30 აპრილი - აზერბაიჯანის გრან პრი (15:00)",
                NumberOfTickets = 500,
            };
        }
    }
}
