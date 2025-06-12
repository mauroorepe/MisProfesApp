using Microsoft.EntityFrameworkCore;
using MisProfesApp.Models;

var builder = WebApplication.CreateBuilder(args);

//Add dbContext
builder.Services.AddDbContext<MisProfesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MisProfessApp")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
