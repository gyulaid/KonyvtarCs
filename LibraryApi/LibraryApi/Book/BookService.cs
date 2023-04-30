using LibraryApi.Database;
using LibraryApi.Exception;

namespace LibraryApi.Book;

public class BookService
{
    private const string BOOK_NOT_FOUND = "Book was not found with id: ";

    private readonly LibraryContext libraryContext;
    private readonly ILogger<BookService> logger;

    public BookService(LibraryContext libraryContext, ILogger<BookService> logger)
    {
        this.libraryContext = libraryContext;
        this.logger = logger;
    }
    
    public List<Book> GetAllBooks()
    {
        return this.libraryContext.Books.ToList();
    }

    public Book GetBookById(int id)
    {
        var book = this.libraryContext.Books.Find(id);
        if (book != null)
        {
            return book;
        }

        throw new EntityNotFoundException(BOOK_NOT_FOUND + id);
    }

    public Book CreateBook(Book book)
    {
        var savedBook = this.libraryContext.Books.Add(book);
        this.libraryContext.SaveChanges();
        this.logger.Log(LogLevel.Information, "A new book was saved");
        return savedBook.Entity;
    }

    public void DeleteBook(int id)
    {
        var book = this.libraryContext.Books.FindAsync(id);
        if (book.Result != null)
        {
            this.libraryContext.Books.Remove(book.Result);
            this.libraryContext.SaveChanges();
        }
        else
        {
            throw new EntityNotFoundException(BOOK_NOT_FOUND + id);
        }
    }
}