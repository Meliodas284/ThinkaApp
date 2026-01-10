namespace Thinka.Domain.Exceptions;

public abstract class ThinkaException : Exception
{
    protected ThinkaException(string message) : base(message)
    {
    }
}
