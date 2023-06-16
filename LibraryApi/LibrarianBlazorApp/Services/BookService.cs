using System.Net.Http.Json;
using LibraryApi.Contracts.Book;

namespace LibrarianBlazorApp.Services;

public class BookService : IBookService
{
    private readonly HttpClient _httpClient;
    
    public BookService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<BookResponseDto>?> GetAllBooksAsync() =>
        await _httpClient.GetFromJsonAsync<IEnumerable<BookResponseDto>>("books");
    
    public async Task<BookLendingDetailsDto?> GetBookByIdAsync(int id) =>
        await _httpClient.GetFromJsonAsync<BookLendingDetailsDto>($"books/{id}/lending-details");



}