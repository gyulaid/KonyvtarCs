using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Book;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string Publisher { get; set; }

    public int PublishYear { get; set; }

    public bool IsAvailable { get; set; }
}