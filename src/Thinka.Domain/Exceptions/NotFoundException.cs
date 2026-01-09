namespace Thinka.Domain.Exceptions;

public class NotFoundException : ThinkaException
{
    public NotFoundException(string message) : base(message)
    {
    }
}
