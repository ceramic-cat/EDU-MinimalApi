using Microsoft.EntityFrameworkCore;
namespace Author.Api.Models;

public class AuthorContext: DbContext
{
    public AuthorContext(DbContextOptions<AuthorContext> options)
       : base(options)
    {
    }

    public DbSet<AuthorModel> Authors { get; set; } = null!;
}
