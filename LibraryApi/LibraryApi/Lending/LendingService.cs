using LibraryApi.Database;

namespace LibraryApi.Lending;

public class LendingService
{
    private readonly LibraryContext _libraryContext;
    private readonly ILogger<LendingService> _logger;

    public LendingService(LibraryContext libraryContext, ILogger<LendingService> logger)
    {
        _libraryContext = libraryContext;
        _logger = logger;
    }
}