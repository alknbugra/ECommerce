namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Şifre hash'leme servisi interface'i
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Şifreyi hash'le
    /// </summary>
    /// <param name="password">Düz metin şifre</param>
    /// <returns>Hash'lenmiş şifre</returns>
    string HashPassword(string password);

    /// <summary>
    /// Şifreyi doğrula
    /// </summary>
    /// <param name="password">Düz metin şifre</param>
    /// <param name="hashedPassword">Hash'lenmiş şifre</param>
    /// <returns>Şifre doğru mu?</returns>
    bool VerifyPassword(string password, string hashedPassword);

    /// <summary>
    /// Şifre güçlülüğünü kontrol et
    /// </summary>
    /// <param name="password">Kontrol edilecek şifre</param>
    /// <returns>Şifre güçlü mü?</returns>
    bool IsPasswordStrong(string password);

    /// <summary>
    /// Şifre güçlülük kurallarını getir
    /// </summary>
    /// <returns>Şifre kuralları</returns>
    IEnumerable<string> GetPasswordRequirements();
}
