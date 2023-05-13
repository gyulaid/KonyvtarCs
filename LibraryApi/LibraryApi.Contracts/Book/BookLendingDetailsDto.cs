using LibraryApi.Contracts.Member;

namespace LibraryApi.Contracts.Book;

public class BookLendingDetailsDto
{
    public BookResponseDto Book { get; set; }
    public MemberResponseDto? Member { get; set; }
    public DateTime? DeadlineOfReturn { get; set; }
}