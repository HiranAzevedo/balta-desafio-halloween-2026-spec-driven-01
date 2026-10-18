using PasswordGenerator.Api.Models;
using PasswordGenerator.Api.Repositories;
using PasswordGenerator.Api.Services;

namespace PasswordGenerator.Tests;

public class PasswordServiceTests
{
    [Fact]
    public async Task GenerateAsync_CreatesAndPersistsPasswordWithGuid()
    {
        var repository = new InMemoryPasswordRepository();
        var service = new PasswordService(repository);

        GeneratedPassword generatedPassword = await service.GenerateAsync();
        GeneratedPassword? storedPassword = await repository.GetByIdAsync(generatedPassword.Id);

        Assert.NotEqual(Guid.Empty, generatedPassword.Id);
        Assert.NotNull(storedPassword);
        Assert.Equal(generatedPassword.Value, storedPassword.Value);
    }

    [Fact]
    public async Task GenerateAsync_AssignsUniqueGuidsWithoutReplacingPreviousPasswords()
    {
        var repository = new InMemoryPasswordRepository();
        var service = new PasswordService(repository);

        GeneratedPassword firstPassword = await service.GenerateAsync();
        GeneratedPassword secondPassword = await service.GenerateAsync();

        Assert.NotEqual(firstPassword.Id, secondPassword.Id);
        Assert.Equal(2, repository.Count);
        Assert.Equal(firstPassword.Value, (await repository.GetByIdAsync(firstPassword.Id))?.Value);
        Assert.Equal(secondPassword.Value, (await repository.GetByIdAsync(secondPassword.Id))?.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenPasswordDoesNotExist()
    {
        var service = new PasswordService(new InMemoryPasswordRepository());

        GeneratedPassword? password = await service.GetByIdAsync(Guid.NewGuid());

        Assert.Null(password);
    }

    private sealed class InMemoryPasswordRepository : IPasswordRepository
    {
        private readonly Dictionary<Guid, GeneratedPassword> passwords = new();

        public int Count => passwords.Count;

        public Task AddAsync(GeneratedPassword password, CancellationToken cancellationToken = default)
        {
            passwords[password.Id] = password;
            return Task.CompletedTask;
        }

        public Task<GeneratedPassword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            passwords.TryGetValue(id, out GeneratedPassword? password);
            return Task.FromResult(password);
        }
    }
}