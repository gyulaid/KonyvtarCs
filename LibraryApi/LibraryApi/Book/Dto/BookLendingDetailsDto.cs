using LibraryApi.Member.Dto;

namespace LibraryApi.Book;

public class BookLendingDetailsDto
{
    public bool IsAvailable { get; set; }
    public MemberResponseDto? Member { get; set; }
    public DateTime? DeadlineOfReturn { get; set; }
}