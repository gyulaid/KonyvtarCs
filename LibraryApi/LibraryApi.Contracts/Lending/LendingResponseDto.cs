namespace LibraryApi.Contracts.Lending;

public class LendingResponseDto
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime DateOfLend { get; set; }
    public DateTime DeadlineOfReturn { get; set; }
    public DateTime? DateOfReturn { get; set; }
}