namespace LibraryApi.Exception;

public class InvalidDateException: System.Exception
{
    public InvalidDateException(string message)
        : base(message)
    {
    }
}