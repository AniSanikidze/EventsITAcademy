using EventsITAcademy.Application.Users.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Infrastructure.SwaggerExamples
{
    public class UserExample : IExamplesProvider<UserResponseModel>
    {
        public UserResponseModel GetExamples()
        {
            return new UserResponseModel
            {
                Id = new Guid().ToString(),
                UserName = "ExampleUser",
                Email = "exampleuser@gmail.com"
            };
        }
    }
}
