using System.Net;

namespace Core.Exceptions;

public sealed class RoomNotFoundException : DomainException
{
    public RoomNotFoundException(int roomId)
        : base($"Room with id {roomId} was not found.", (int)HttpStatusCode.NotFound)
    {
    }
}