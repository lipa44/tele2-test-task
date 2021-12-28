namespace Domain.Exceptions;

public class Tele2Exception : Exception
{
    public Tele2Exception() { }

    public Tele2Exception(string message)
        : base(message) { }

    public Tele2Exception(string message, Exception innerException)
        : base(message, innerException) { }
}