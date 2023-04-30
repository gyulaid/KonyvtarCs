namespace LibraryApi.Exception;

public class EntityNotFoundException : System.Exception
{
    public EntityNotFoundException(string message)
        : base(message)
    {
    }
}