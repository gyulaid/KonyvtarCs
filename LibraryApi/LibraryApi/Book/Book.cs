using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Book;

public class Book
{
    public string title { get; set; }
    public string author { get; set; }
    public string publisher { get; set; }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int publishYear { get; set; }
}