using LibraryApi.Database;

namespace LibraryApi.Book;

public class BookService
{
    private readonly LibraryContext _libraryContext;
    private readonly ILogger<BookService> _logger;

    public BookService(LibraryContext libraryContext, ILogger<BookService> logger)
    {
        _libraryContext = libraryContext;
        _logger = logger;
    }
}