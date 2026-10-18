using PasswordGenerator.Api.Models;

namespace PasswordGenerator.Api.Services;

public interface IPasswordService
{
    Task<GeneratedPassword> GenerateAsync(CancellationToken cancellationToken = default);

    Task<GeneratedPassword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}