namespace Thinka.Domain.Exceptions;

public class ConflictException : ThinkaException
{
    public ConflictException(string message) : base(message)
    {
    }
}
