using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Data;
using MoviesApi.Filter;
using MoviesApi.Helpers;
using MoviesApi.Repository.Implementation;
using MoviesApi.Repository.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console().CreateLogger();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("No Connection String was found");

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddCors();
try
{

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
    //app.UseCors();

    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var error = context.Features.Get<IExceptionHandlerFeature>();
            var exception = error?.Error;
            Log.Logger.Error(exception, "Unhandled exception occurred");
        });
    });

    app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

    Log.Logger.Information("Application is running ....");
app.Run();

}
catch(Exception ex)
{
    Log.Logger.Error(ex, "Application failed to start....");
}
finally
{
    Log.CloseAndFlush();
}