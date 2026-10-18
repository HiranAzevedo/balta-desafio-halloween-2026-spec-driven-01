using PasswordGeneratorService = PasswordGenerator.Api.Services.PasswordGenerator;

namespace PasswordGenerator.Tests;

public class PasswordGeneratorTests
{
    private const string AllowedCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()-_=+[]{};:,.?";
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.?";

    [Fact]
    public void Generate_ReturnsExactlySixteenCharacters()
    {
        string password = PasswordGeneratorService.Generate();

        Assert.Equal(16, password.Length);
    }

    [Fact]
    public void Generate_ContainsAtLeastOneSpecialCharacter()
    {
        string password = PasswordGeneratorService.Generate();

        Assert.Contains(password, character => SpecialCharacters.Contains(character));
    }

    [Fact]
    public void Generate_ContainsNoWhitespace()
    {
        string password = PasswordGeneratorService.Generate();

        Assert.DoesNotContain(password, char.IsWhiteSpace);
    }

    [Fact]
    public void Generate_UsesOnlyAllowedCharacters()
    {
        string password = PasswordGeneratorService.Generate();

        Assert.All(password, character => Assert.Contains(character, AllowedCharacters));
    }
}