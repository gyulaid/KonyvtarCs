namespace LibraryApi.Contracts.Book;

public class CreateBookDto
{
    public string title { get; set; }
    public string author { get; set; }
    public string publisher { get; set; }
    public int publishYear { get; set; }
}