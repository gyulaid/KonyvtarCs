namespace LibraryApi.Lending.Dto;

public class CreateLendingDto
{
    public int MemberId { get; set; }
    public int BookId { get; set; }
    public DateTime dateOfLend { get; set; }
    public DateTime? dateOfReturn { get; set; }
}