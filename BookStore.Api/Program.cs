using BookStore.Api;
using BookStore.Lib;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookStoreDb>(opt => opt.UseInMemoryDatabase("Bookstore"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// GET all
app.MapGet("/books", async (BookStoreDb db) =>
    await db.Books.ToListAsync());

// GET id
app.MapGet("/books/{id}", async (int id, BookStoreDb db) =>
    await db.Books.FindAsync(id)
        is Book book
            ? Results.Ok(book)
            : Results.NotFound());

// GET by author
app.MapGet("/books/author/{author}", async (string author, BookStoreDb db) =>
{
    var matchingBooks = await db.Books
    .Where(b => b.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
    .ToListAsync();

    return matchingBooks.Any() ? Results.Ok(matchingBooks) : Results.NotFound("No books found by that author.");
}
    );


// Post all
app.MapPost("/books", async (Book book, BookStoreDb db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Created($"/books/{book.Id}", book);
});

// Post matching id
app.MapPut("/books/{id}", async (int id, Book inputBook, BookStoreDb db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return Results.NotFound();

    book.Title = inputBook.Title;
    book.Author = inputBook.Author;
    book.Review = inputBook.Review;
    book.Description = inputBook.Description;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Patch review matching id
app.MapPatch("/books/{id}", async (int id, string review, BookStoreDb db) =>
{
    var book = await db.Books.FindAsync(id);
    if (book is null) return Results.NotFound();

    book.Review = review;
    await db.SaveChangesAsync();

    return Results.Accepted();

});



// Delete matching id
app.MapDelete("/books/{id}", async (int id, BookStoreDb db) =>
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();