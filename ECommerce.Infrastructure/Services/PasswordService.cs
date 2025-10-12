using ECommerce.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace ECommerce.Infrastructure.Services;

/// <summary>
/// Şifre hash'leme servisi implementasyonu
/// </summary>
public class PasswordService : IPasswordService
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 10000;

    /// <summary>
    /// Şifreyi hash'le
    /// </summary>
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Şifre boş olamaz.", nameof(password));

        // Salt oluştur
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        // Şifreyi hash'le
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var key = pbkdf2.GetBytes(KeySize);

        // Salt + Key'i birleştir
        var hashBytes = new byte[SaltSize + KeySize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(key, 0, hashBytes, SaltSize, KeySize);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Şifreyi doğrula
    /// </summary>
    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
            return false;

        try
        {
            // Hash'den salt ve key'i çıkar
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Gelen şifreyi aynı salt ile hash'le
            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(KeySize);

            // Hash'leri karşılaştır
            for (int i = 0; i < KeySize; i++)
            {
                if (hashBytes[i + SaltSize] != key[i])
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Şifre güçlülüğünü kontrol et
    /// </summary>
    public bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        // En az 8 karakter
        if (password.Length < 8)
            return false;

        // En az bir büyük harf
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return false;

        // En az bir küçük harf
        if (!Regex.IsMatch(password, @"[a-z]"))
            return false;

        // En az bir rakam
        if (!Regex.IsMatch(password, @"[0-9]"))
            return false;

        // En az bir özel karakter
        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
            return false;

        return true;
    }

    /// <summary>
    /// Şifre güçlülük kurallarını getir
    /// </summary>
    public IEnumerable<string> GetPasswordRequirements()
    {
        return new List<string>
        {
            "En az 8 karakter uzunluğunda olmalı",
            "En az bir büyük harf içermeli",
            "En az bir küçük harf içermeli",
            "En az bir rakam içermeli",
            "En az bir özel karakter içermeli (!@#$%^&*()_+-=[]{}|;':\",./<>?)"
        };
    }
}
