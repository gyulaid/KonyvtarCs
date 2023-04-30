namespace LibraryApi.Member.Dto;

public class CreateMemberDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
}