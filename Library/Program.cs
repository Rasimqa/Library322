using Microsoft.EntityFrameworkCore;
using Library.DBContext;
using Library.Interfaces;
using Library.Service;
using ProxyKit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<APIDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIString")), ServiceLifetime.Scoped);

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddScoped<IReaderService, ReaderService>();

builder.Services.AddScoped<IRentService, RentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseWhen(context => context.Request.Path.Value.Contains("/api/Books"),
    applicationBuilder => applicationBuilder.RunProxy(context =>
        context.ForwardTo("https://localhost:7003").AddXForwardedHeaders().Send()));
app.UseWhen(context => context.Request.Path.Value.Contains("/api/Genres"),
    applicationBuilder => applicationBuilder.RunProxy(context =>
        context.ForwardTo("https://localhost:7095").AddXForwardedHeaders().Send()));
app.UseWhen(context => context.Request.Path.Value.Contains("/api/Rent"),
    applicationBuilder => applicationBuilder.RunProxy(context =>
        context.ForwardTo("https://localhost:7009").AddXForwardedHeaders().Send()));
app.UseWhen(context => context.Request.Path.Value.Contains("/api/Readers"),
    applicationBuilder => applicationBuilder.RunProxy(context =>
        context.ForwardTo("https://localhost:7234").AddXForwardedHeaders().Send()));


app.MapControllers();

app.Run();
