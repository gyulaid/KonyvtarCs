using LibrarianBlazorApp.Pages;
using LibraryApi.Contracts.Book;
using LibraryApi.Contracts.Member;

namespace LibrarianBlazorApp.Services
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberResponseDto>?> GetAllMembersAsync();

        Task<MemberResponseDto?> GetMemberByIdAsync(int id);

        Task<IEnumerable<BookLendingDetailsDto>?> GetBooksLentByMember(int id);

        Task AddMemberAsync(CreateMemberDto member);
    }
}