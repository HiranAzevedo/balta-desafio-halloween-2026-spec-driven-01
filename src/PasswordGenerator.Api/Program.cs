using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Endpoints;
using PasswordGenerator.Api.Repositories;
using PasswordGenerator.Api.Services;

var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("PasswordGenerator")
	?? throw new InvalidOperationException("A connection string 'PasswordGenerator' não está configurada.");

builder.Services.AddDbContext<PasswordGeneratorDbContext>(options =>
	options.UseSqlite(connectionString));
builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
	PasswordGeneratorDbContext dbContext = scope.ServiceProvider.GetRequiredService<PasswordGeneratorDbContext>();
	await dbContext.Database.MigrateAsync();
}

app.MapPasswordEndpoints();
app.Run();

public partial class Program { }
