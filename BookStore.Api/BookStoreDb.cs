using Microsoft.EntityFrameworkCore;
using BookStore.Lib;
namespace BookStore.Api;

public class BookStoreDb : DbContext
{
    public BookStoreDb(DbContextOptions<BookStoreDb> options) : base(options) { }
    public DbSet<Book> Books => Set<Book>();

}
