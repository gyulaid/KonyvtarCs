namespace LibraryApi.Book;

public class BookResponseDto
{
    public int Id { get; set; }
    public string title { get; set; }
    public string author { get; set; }
    public int publishYear { get; set; }
    public string publisher { get; set; }
}