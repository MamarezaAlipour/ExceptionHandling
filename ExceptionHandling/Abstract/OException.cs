namespace ExceptionHandling;

public class OException : Exception
{
    public static string DefaultMessage { get; set; } =
        "error occured when execute the current request. please try again later or contact the administrator.";

    public OException() : base(DefaultMessage)
    {
    }


    public OException(string message) : base(string.IsNullOrWhiteSpace(message) ? DefaultMessage : message)
    {
    }

    public OException(string message, Exception innerException)
        : base(string.IsNullOrWhiteSpace(message) ? DefaultMessage : message, innerException)
    {
    }
}
