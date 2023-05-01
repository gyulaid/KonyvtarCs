using LibraryApi.Member.Dto;

namespace LibraryApi.Book;

public class BookLendingDetailsDto
{
    public Book Book { get; set; }
    public MemberResponseDto? Member { get; set; }
    public DateTime? DeadlineOfReturn { get; set; }
}