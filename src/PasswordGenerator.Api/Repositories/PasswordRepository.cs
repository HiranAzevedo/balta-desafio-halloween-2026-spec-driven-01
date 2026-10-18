using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Data;
using PasswordGenerator.Api.Models;

namespace PasswordGenerator.Api.Repositories;

public sealed class PasswordRepository(PasswordGeneratorDbContext dbContext) : IPasswordRepository
{
    public async Task AddAsync(GeneratedPassword password, CancellationToken cancellationToken = default)
    {
        await dbContext.GeneratedPasswords.AddAsync(password, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<GeneratedPassword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext.GeneratedPasswords
            .SingleOrDefaultAsync(password => password.Id == id, cancellationToken);
    }
}