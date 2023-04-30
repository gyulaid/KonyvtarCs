using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Lending;

public class Lending
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public virtual Member.Member Member { get; set; }

    public virtual Book.Book Book { get; set; }

    public DateTime DateOfLend { get; set; }

    public DateTime DeadlineOfReturn { get; set; }

    public DateTime? DateOfReturn { get; set; }
}