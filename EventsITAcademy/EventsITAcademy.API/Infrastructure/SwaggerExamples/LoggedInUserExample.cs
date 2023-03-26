using EventsITAcademy.Application.Users.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Infrastructure.SwaggerExamples
{
    public class LoggedInUserExample : IExamplesProvider<LoggedInUserResponseModel>
    {
        public LoggedInUserResponseModel GetExamples()
        {
            return new LoggedInUserResponseModel
            {
                Id = new Guid().ToString(),
                UserName = "ExampleUser",
                Email = "exampleuser@gmail.com",
                Token = "exampletoken",
                Role = "User"
            };
        }
    }
}
