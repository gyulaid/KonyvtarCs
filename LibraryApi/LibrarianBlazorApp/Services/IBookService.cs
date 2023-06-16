using LibraryApi.Contracts.Book;

namespace LibrarianBlazorApp.Services;


public interface IBookService
{
    Task<IEnumerable<BookResponseDto>?> GetAllBooksAsync();

    Task<BookLendingDetailsDto?> GetBookByIdAsync(int id);
}