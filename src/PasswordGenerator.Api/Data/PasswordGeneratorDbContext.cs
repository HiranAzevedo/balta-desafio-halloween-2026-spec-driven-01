using Microsoft.EntityFrameworkCore;
using PasswordGenerator.Api.Models;

namespace PasswordGenerator.Api.Data;

public sealed class PasswordGeneratorDbContext(DbContextOptions<PasswordGeneratorDbContext> options)
    : DbContext(options)
{
    public DbSet<GeneratedPassword> GeneratedPasswords => Set<GeneratedPassword>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GeneratedPassword>(entity =>
        {
            entity.HasKey(password => password.Id);
            entity.Property(password => password.Value).IsRequired();
        });
    }
}