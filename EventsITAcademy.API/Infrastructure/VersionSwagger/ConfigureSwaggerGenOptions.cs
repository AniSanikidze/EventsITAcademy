using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EventsITAcademy.API.Infrastructure.VersionSwagger
{
    public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider apiVersionDescriptionProvider)
            => _apiVersionDescriptionProvider = apiVersionDescriptionProvider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateOpenApiInfo(description));
            }
        }

        private static OpenApiInfo CreateOpenApiInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Events IT Academy API",
                Version = description.ApiVersion.ToString(),
                Description = "Api to manage Events",
                Contact = new OpenApiContact
                {
                    Email = "itacademy@tbcbank.com",
                    Name = "Events IT Academy",
                    Url = new Uri("https://tbcacademy.ge")
                }
            };

            if (description.IsDeprecated)
            {
                info.Description += " (deprecated)";
            }

            return info;
        }

    }
}
