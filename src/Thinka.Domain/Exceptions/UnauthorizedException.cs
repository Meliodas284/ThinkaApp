namespace Thinka.Domain.Exceptions;

public class UnauthorizedException : ThinkaException
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}
