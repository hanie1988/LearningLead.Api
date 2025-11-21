using System.Net;

namespace Core.Exceptions;

public sealed class RoomNotAvailableException : DomainException
{
    public RoomNotAvailableException()
        : base("Room is not available for the selected dates.", (int)HttpStatusCode.UnprocessableEntity)
    {
    }
}