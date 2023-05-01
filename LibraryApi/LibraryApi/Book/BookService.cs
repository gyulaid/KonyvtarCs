using AutoMapper;
using LibraryApi.Database;
using LibraryApi.Exception;
using LibraryApi.Member.Dto;
using Microsoft.EntityFrameworkCore;

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

    public List<BookResponseDto> GetAllBooksLent()
    {
        return mapper.Map<List<BookResponseDto>>(
            this.libraryContext.Books
                .Where(book => !book.IsAvailable)
                .ToList()
        );
    }

    public List<BookResponseDto> GetAllAvailableBooks()
    {
        return mapper.Map<List<BookResponseDto>>(
            this.libraryContext.Books
                .Where(book => book.IsAvailable)
                .ToList()
        );
    }

    public BookLendingDetailsDto GetBookLendingDetails(int id)
    {
        var book = libraryContext.Books.Find(id);
        if (book is null)
        {
            throw new EntityNotFoundException(BookNotFound + id);
        }

        var details = new BookLendingDetailsDto();

        var lending = this.FindActiveLendingByBookId(id);

        if (lending is null)
        {
            details.IsAvailable = true;
            details.Member = null;
            details.DeadlineOfReturn = null;

            return details;
        }

        details.IsAvailable = false;
        details.Member = this.mapper.Map<MemberResponseDto>(lending.Member);
        details.DeadlineOfReturn = lending.DeadlineOfReturn;

        return details;
    }

    private Lending.Lending FindActiveLendingByBookId(int id)
    {
        return this.libraryContext.Lendings
            .Include(include => include.Member)
            .Join(
                libraryContext.Lendings,
                lending => lending.Book.Id,
                book => book.Id,
                (lending, book) => new { Lending = lending, Book = book })
            .Where(joinedResult => !joinedResult.Book.Book.IsAvailable && joinedResult.Book.Id == id)
            .Select(result => result.Lending)
            .First();
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