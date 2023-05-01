namespace LibraryApi.Exception;

public class NotAvailableException: System.Exception
{
    public NotAvailableException(string message)
        : base(message)
    {
    }
}