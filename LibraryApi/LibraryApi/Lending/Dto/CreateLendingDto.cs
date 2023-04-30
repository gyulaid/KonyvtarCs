namespace LibraryApi.Lending.Dto;

public class CreateLendingDto
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime DateOfLend { get; set; }
    public DateTime DeadlineOfReturn { get; set; }
}