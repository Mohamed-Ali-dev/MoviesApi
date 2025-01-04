using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Filter;
using MoviesApi.Repository.Implementation;
using MoviesApi.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("No Connection String was found");

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
