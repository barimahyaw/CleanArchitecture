using Domain.Exceptions.Base;

namespace Domain.Exceptions;

public sealed class WebinarNotFoundException : NotFoundException
{
    public WebinarNotFoundException(Guid webinarId)
        : base($"The webinar with identifier {webinarId} was not found.")
    {
    }
}