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

    [HttpGet("lent/member/{memberId:int}")]
    public List<BookLendingDetailsDto> GetBookLendingDetailsByCurrentMember(int memberId)
    {
        return this.bookService.GetLendingDetailsOfBooksByMemberId(memberId);
    }

    [HttpGet("available")]
    public List<BookResponseDto> GetAllAvailableBooks()
    {
        return this.bookService.GetAllAvailableBooks();
    }

    [HttpGet("{id:int}/lending-details")]
    public BookLendingDetailsDto GetBookLendingDetails(int id)
    {
        return this.bookService.GetBookLendingDetails(id);
    }

    [HttpGet("{id:int}")]
    public BookResponseDto GetBook(int id)
    {
        return this.bookService.GetBookById(id);
    }

    [HttpPost]
    public BookResponseDto CreateBook([FromBody] CreateBookDto createDto)
    {
        return this.bookService.CreateBook(createDto);
    }

    [HttpDelete("{id:int}")]
    public void DeleteBook(int id)
    {
        this.bookService.DeleteBook(id);
    }
}