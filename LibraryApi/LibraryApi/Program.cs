using LibraryApi.Book;
using LibraryApi.Database;
using LibraryApi.Lending;
using LibraryApi.Member;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<LendingService>();
builder.Services.AddDbContext<LibraryContext>(
    options =>
    {
        options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"));
    }
);
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
