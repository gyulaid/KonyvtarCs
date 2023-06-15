using System.Net.Http.Json;
using LibrarianBlazorApp.Pages;
using LibraryApi.Contracts.Book;
using LibraryApi.Contracts.Member;

namespace LibrarianBlazorApp.Services
{
    public class MemberService : IMemberService
    {
        private readonly HttpClient _httpClient;
        
        public MemberService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<IEnumerable<MemberResponseDto>?> GetAllMembersAsync() =>
            _httpClient.GetFromJsonAsync<IEnumerable<MemberResponseDto>>("Members");

        public Task<MemberResponseDto?> GetMemberByIdAsync(int id) =>
            _httpClient.GetFromJsonAsync<MemberResponseDto?>($"Members/{id}");

        public Task<IEnumerable<BookLendingDetailsDto>?> GetBooksLentByMember(int id) =>
            _httpClient.GetFromJsonAsync<IEnumerable<BookLendingDetailsDto>>($"books/lent/member/{id}");
    }
}