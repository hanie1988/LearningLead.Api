using FluentValidation;

namespace Application.Rooms.Validators;

public sealed class RoomCreateDtoValidator : AbstractValidator<RoomCreateDto>
{
    public RoomCreateDtoValidator()
    {
        RuleFor(x => x.HotelId)
            .GreaterThan(0);

        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Capacity)
            .GreaterThan(0);

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0);
    }
}