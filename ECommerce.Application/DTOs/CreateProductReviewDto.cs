namespace ECommerce.Application.DTOs;

/// <summary>
/// Ürün değerlendirmesi oluşturma DTO'su
/// </summary>
public class CreateProductReviewDto
{
    /// <summary>
    /// Ürün ID'si
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Sipariş ID'si (opsiyonel - doğrulama için)
    /// </summary>
    public Guid? OrderId { get; set; }

    /// <summary>
    /// Sipariş detayı ID'si (opsiyonel)
    /// </summary>
    public Guid? OrderItemId { get; set; }

    /// <summary>
    /// Puan (1-5 arası)
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Değerlendirme başlığı
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Değerlendirme içeriği
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// Değerlendirme türü
    /// </summary>
    public string ReviewType { get; set; } = "Verified";

    /// <summary>
    /// Resim dosyaları (base64 veya file upload)
    /// </summary>
    public List<string>? ImageFiles { get; set; }
}
