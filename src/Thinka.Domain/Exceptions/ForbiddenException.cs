namespace Thinka.Domain.Exceptions;

public class ForbiddenException : ThinkaException
{
    public ForbiddenException(string message) : base(message)
    {
    }
}
