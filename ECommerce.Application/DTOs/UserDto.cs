namespace ECommerce.Application.DTOs;

/// <summary>
/// Kullanıcı DTO'su
/// </summary>
public class UserDto
{
    /// <summary>
    /// Kullanıcı ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcı adı (Email'den türetilir)
    /// </summary>
    public string UserName => Email;

    /// <summary>
    /// E-posta adresi
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Ad
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Soyad
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Telefon numarası
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// E-posta doğrulandı mı?
    /// </summary>
    public bool EmailConfirmed { get; set; }

    /// <summary>
    /// Telefon doğrulandı mı?
    /// </summary>
    public bool PhoneNumberConfirmed { get; set; }

    /// <summary>
    /// Hesap kilitli mi?
    /// </summary>
    public bool IsLocked { get; set; }

    /// <summary>
    /// Kullanıcı aktif mi?
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Tam adı
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Kullanıcı rolleri
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
