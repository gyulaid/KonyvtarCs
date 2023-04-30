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
    public List<BookResponseDto> GetAllBooks()
    {
        return this.bookService.GetAllBooks();
    }

    [HttpGet("{id}")]
    public BookResponseDto GetBook(int id)
    {
        return this.bookService.GetBookById(id);
    }

    [HttpPost]
    public BookResponseDto CreateBook([FromBody] CreateBookDto createDto)
    {
        return this.bookService.CreateBook(createDto);
    }

    [HttpDelete("{id}")]
    public void DeleteBook(int id)
    {
        this.bookService.DeleteBook(id);
    }
}