using Microsoft.EntityFrameworkCore;
using Library.DBContext;
using Library.Interfaces;
using Library.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<APIDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIString")), ServiceLifetime.Scoped);

builder.Services.AddScoped<IGenreService, GenreServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
