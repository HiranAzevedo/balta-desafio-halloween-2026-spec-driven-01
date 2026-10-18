using PasswordGenerator.Api.Models;
using PasswordGenerator.Api.Repositories;

namespace PasswordGenerator.Api.Services;

public sealed class PasswordService(IPasswordRepository passwordRepository) : IPasswordService
{
    public async Task<GeneratedPassword> GenerateAsync(CancellationToken cancellationToken = default)
    {
        var generatedPassword = new GeneratedPassword
        {
            Id = Guid.NewGuid(),
            Value = PasswordGenerator.Generate()
        };

        await passwordRepository.AddAsync(generatedPassword, cancellationToken);
        return generatedPassword;
    }

    public Task<GeneratedPassword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return passwordRepository.GetByIdAsync(id, cancellationToken);
    }
}