namespace PasswordGenerator.Api.Models;

public sealed class GeneratedPassword
{
    public Guid Id { get; set; }

    public string Value { get; set; } = string.Empty;
}