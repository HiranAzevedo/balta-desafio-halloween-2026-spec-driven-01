namespace PasswordGenerator.Api.Contracts;

/// <summary>Represents a generated password and its lookup identifier.</summary>
public sealed record PasswordResponse(Guid Id, string Password);