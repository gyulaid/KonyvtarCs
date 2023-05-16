using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Database;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual DbSet<Book.Book> Books { get; set; }
    public virtual DbSet<Member.Member> Members { get; set; }
    public virtual DbSet<Lending.Lending> Lendings { get; set; }
}