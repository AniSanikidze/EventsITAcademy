// Copyright (C) TBC Bank. All Rights Reserved.

using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace EventsITAcademy.API.Infrastructure.SwaggerExamples
{
    public class UserExamples : IMultipleExamplesProvider<UserResponseModel>
    {
        public IEnumerable<SwaggerExample<UserResponseModel>> GetExamples()
        {
            yield return SwaggerExample.Create("example 1", new UserResponseModel
            {
                Id = new Guid().ToString(),
                UserName = "ExampleUser",
                Email = "exampleuser@gmail.com",
            });

            yield return SwaggerExample.Create("example 2", new UserResponseModel
            {
                Id = new Guid().ToString(),
                UserName = "ExampleUser1",
                Email = "exampleuser1@gmail.com",
            });
        }
    }
}
