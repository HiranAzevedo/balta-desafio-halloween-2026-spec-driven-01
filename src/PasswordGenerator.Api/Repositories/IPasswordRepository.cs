using PasswordGenerator.Api.Models;

namespace PasswordGenerator.Api.Repositories;

public interface IPasswordRepository
{
    Task AddAsync(GeneratedPassword password, CancellationToken cancellationToken = default);

    Task<GeneratedPassword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}