using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Models;
using PasswordGenerator.Api.Repositories;

namespace PasswordGenerator.Tests;

public class PasswordRepositoryTests
{
    [Fact]
    public async Task AddAsync_PersistsPasswordThatCanBeRetrievedById()
    {
        await using var connection = new SqliteConnection("Data Source=:memory:");
        await connection.OpenAsync();

        DbContextOptions<PasswordGeneratorDbContext> options = new DbContextOptionsBuilder<PasswordGeneratorDbContext>()
            .UseSqlite(connection)
            .Options;
        var password = new GeneratedPassword { Id = Guid.NewGuid(), Value = "Abc123!@#$xyz789" };

        await using (var writeContext = new PasswordGeneratorDbContext(options))
        {
            await writeContext.Database.EnsureCreatedAsync();
            var repository = new PasswordRepository(writeContext);

            await repository.AddAsync(password);
        }

        await using var readContext = new PasswordGeneratorDbContext(options);
        var readRepository = new PasswordRepository(readContext);

        GeneratedPassword? retrievedPassword = await readRepository.GetByIdAsync(password.Id);

        Assert.NotNull(retrievedPassword);
        Assert.Equal(password.Value, retrievedPassword.Value);
    }
}