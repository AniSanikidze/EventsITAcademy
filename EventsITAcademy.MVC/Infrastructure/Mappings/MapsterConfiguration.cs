﻿using EventsITAcademy.Application.Events.Requests;
using EventsITAcademy.Application.Events.Responses;
using EventsITAcademy.Application.Users.Responses;
using EventsITAcademy.Domain.Events;
using EventsITAcademy.Domain.Users;
using Mapster;
using System.Net;

namespace EventsITAcademy.MVC.Infrastructure.Mappings
{
    public static class MapsterConfiguration
    {
        public static void RegisterMaps(this IServiceCollection services)
        {
            TypeAdapterConfig<Event, EventResponseModel>
                .NewConfig()
                .Map(dest => dest.ImageDataUrl, src => src.Image.ImageDataUrl);
            //TypeAdapterConfig<EventResponseModel, UserUpdateEventRequestModel>
            //    .NewConfig()
            //    .Map(dest => dest.ImageFile.FileName, src => src.Image.ImageName);
                //.Map(dest => dest.ImageFile., src => src.Image.ImageName);

        }
    }
}
