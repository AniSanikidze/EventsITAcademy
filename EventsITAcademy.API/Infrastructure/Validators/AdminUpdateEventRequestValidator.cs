﻿using EventsITAcademy.API.Infrastructure.Localizations;
using EventsITAcademy.Application.Events.Requests;
using FluentValidation;

namespace EventsITAcademy.API.Infrastructure.Validators
{
    public class AdminUpdateEventRequestValidator : AbstractValidator<AdminUpdateEventRequestModel>
    {
        public AdminUpdateEventRequestValidator()
        {
            RuleFor(x => x.Title)
                     .Must(x => x.Length > 0 && x.Length <= 100)
                     .NotEmpty()
                     .WithMessage(ValidationErrorMessages.EventTitleLength);

            RuleFor(x => x.NumberOfTickets)
                    .GreaterThan(0)
                    .WithMessage(ValidationErrorMessages.NumberOfTicketsRequired);

            RuleFor(x => x.Description)
                    .Must(desc => desc.Length > 0 && desc.Length <= 100)
                    .WithMessage(ValidationErrorMessages.DescMaxLength);

            RuleFor(x => x.StartDate)
                    .Must(x => x > DateTime.Now)
                    .WithMessage(ValidationErrorMessages.StartDateGreaterThanNow);

            RuleFor(x => x.FinishDate)
                    .Must(x => x > DateTime.Now)
                    .WithMessage(ValidationErrorMessages.FinishDateGreaterThanNow);

            RuleFor(x => x.ReservationPeriod)
                .NotEmpty()
                .WithMessage(ValidationErrorMessages.ReservationPeriodRequired);

            RuleFor(x => x.ModificationPeriod)
                .NotEmpty()
                .WithMessage(ValidationErrorMessages.ModificationPeriodRequired);
        }

    }
}
