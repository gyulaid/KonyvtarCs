using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Lending;

public class Lending
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int BookId { get; set; }

    public DateTime DateOfLend { get; set; }

    public DateTime? DateOfReturn { get; set; }
}