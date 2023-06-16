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

        public async Task<IEnumerable<MemberResponseDto>?> GetAllMembersAsync() => 
            await _httpClient.GetFromJsonAsync<IEnumerable<MemberResponseDto>>("Members");

        public async Task<MemberResponseDto?> GetMemberByIdAsync(int id) =>
            await _httpClient.GetFromJsonAsync<MemberResponseDto?>($"Members/{id}");

        public async Task<IEnumerable<BookLendingDetailsDto>?> GetBooksLentByMember(int id) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<BookLendingDetailsDto>>($"books/lent/member/{id}");

        public async Task AddMemberAsync(CreateMemberDto member) =>
            await _httpClient.PostAsJsonAsync("members",member);
    }
}