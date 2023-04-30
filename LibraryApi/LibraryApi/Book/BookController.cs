using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Book;

[ApiController]
[Route("/books")]
public class BookController : ControllerBase
{
    private readonly BookService bookService;

    public BookController(BookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    public List<Book> GetAllBooks()
    {
        return this.bookService.GetAllBooks();
    }

    [HttpGet("{id}")]
    public Book GetBook(int id)
    {
        return this.bookService.GetBookById(id);
    }

    [HttpPost]
    public Book CreateBook([FromBody] Book book)
    {
        return this.bookService.CreateBook(book);
    }

    [HttpDelete("{id}")]
    public void DeleteBook(int id)
    {
        this.bookService.DeleteBook(id);
    }
}