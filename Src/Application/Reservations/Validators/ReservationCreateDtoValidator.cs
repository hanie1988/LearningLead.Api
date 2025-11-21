using FluentValidation;

namespace Application.Reservations.Validators;

public sealed class ReservationCreateDtoValidator : AbstractValidator<ReservationCreateDto>
{
    public ReservationCreateDtoValidator()
    {
        RuleFor(x => x.RoomId)
            .GreaterThan(0)
            .WithMessage("Invalid RoomId.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("Invalid UserId.");

        RuleFor(x => x.CheckIn)
            .NotEmpty()
            .WithMessage("Check-in date is required.");

        RuleFor(x => x.CheckOut)
            .NotEmpty()
            .WithMessage("Check-out date is required.");

        RuleFor(x => x)
            .Must(x => x.CheckIn < x.CheckOut)
            .WithMessage("Check-out must be after check-in.");
    }
}