using System.Net;

namespace Core.Exceptions;

public sealed class InvalidDateRangeException : DomainException
{
    public InvalidDateRangeException()
        : base("Invalid check-in/check-out date range.", (int)HttpStatusCode.BadRequest)
    {
    }
}