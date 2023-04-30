using AutoMapper;
using LibraryApi.Database;
using LibraryApi.Exception;

namespace LibraryApi.Book;

public class BookService
{
    private const string BookNotFound = "Book was not found with id: ";

    private readonly LibraryContext libraryContext;
    private readonly ILogger<BookService> logger;
    private readonly IMapper mapper;

    public BookService(LibraryContext libraryContext, ILogger<BookService> logger, IMapper mapper)
    {
        this.libraryContext = libraryContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public List<BookResponseDto> GetAllBooks()
    {
        return mapper.Map<List<BookResponseDto>>(this.libraryContext.Books.ToList());
    }

    public BookResponseDto GetBookById(int id)
    {
        var book = this.libraryContext.Books.Find(id);
        if (book != null)
        {
            return mapper.Map<BookResponseDto>(book);
        }

        throw new EntityNotFoundException(BookNotFound + id);
    }

    public BookResponseDto CreateBook(CreateBookDto createDto)
    {
        var savedBook = this.libraryContext.Books.Add(mapper.Map<Book>(createDto));
        this.libraryContext.SaveChanges();
        this.logger.Log(LogLevel.Information, "A new book was saved");
        return mapper.Map<BookResponseDto>(savedBook.Entity);
    }

    public void DeleteBook(int id)
    {
        var book = this.libraryContext.Books.FindAsync(id);
        if (book.Result != null)
        {
            this.libraryContext.Books.Remove(book.Result);
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "Book was deleted with id: " + id);
        }
        else
        {
            throw new EntityNotFoundException(BookNotFound + id);
        }
    }
}