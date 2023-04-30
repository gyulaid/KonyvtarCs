using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryApi.Lending;

public class Lending
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int inventoryId { get; set; }

    public DateTime dateOfLend { get; set; }

    public DateTime? dateOfReturn { get; set; }
}