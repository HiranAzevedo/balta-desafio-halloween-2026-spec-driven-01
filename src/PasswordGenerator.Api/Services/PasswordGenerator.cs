using System.Security.Cryptography;

namespace PasswordGenerator.Api.Services;

public static class PasswordGenerator
{
    private const int PasswordLength = 16;
    private const string AlphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const string SpecialCharacters = "!@#$%^&*()-_=+[]{};:,.?";
    private const string AllowedCharacters = AlphanumericCharacters + SpecialCharacters;

    public static string Generate()
    {
        Span<char> password = stackalloc char[PasswordLength];
        password[0] = SpecialCharacters[RandomNumberGenerator.GetInt32(SpecialCharacters.Length)];

        for (int index = 1; index < password.Length; index++)
        {
            password[index] = AllowedCharacters[RandomNumberGenerator.GetInt32(AllowedCharacters.Length)];
        }

        for (int currentIndex = password.Length - 1; currentIndex > 0; currentIndex--)
        {
            int swapIndex = RandomNumberGenerator.GetInt32(currentIndex + 1);
            char temporaryCharacter = password[currentIndex];
            password[currentIndex] = password[swapIndex];
            password[swapIndex] = temporaryCharacter;
        }

        return new string(password);
    }
}