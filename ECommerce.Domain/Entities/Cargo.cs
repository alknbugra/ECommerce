using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

/// <summary>
/// Kargo entity'si
/// </summary>
public class Cargo : BaseEntity
{
    /// <summary>
    /// Kargo takip numarası
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string TrackingNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Kargo şirketi ID'si
    /// </summary>
    public Guid CargoCompanyId { get; set; }

    /// <summary>
    /// Kargo durumu
    /// </summary>
    public CargoStatus Status { get; set; } = CargoStatus.Created;

    /// <summary>
    /// Kargo türü
    /// </summary>
    public CargoType Type { get; set; } = CargoType.Standard;

    /// <summary>
    /// Kargo ağırlığı (kg)
    /// </summary>
    [Column(TypeName = "decimal(8,2)")]
    public decimal Weight { get; set; }

    /// <summary>
    /// Kargo boyutları (cm) - Uzunluk x Genişlik x Yükseklik
    /// </summary>
    [MaxLength(50)]
    public string? Dimensions { get; set; }

    /// <summary>
    /// Kargo ücreti
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal ShippingCost { get; set; }

    /// <summary>
    /// Kargo ücreti (müşteriye yansıtılan)
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal CustomerShippingCost { get; set; }

    /// <summary>
    /// Kargo içeriği açıklaması
    /// </summary>
    [MaxLength(500)]
    public string? ContentDescription { get; set; }

    /// <summary>
    /// Kargo değeri
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? DeclaredValue { get; set; }

    /// <summary>
    /// Kargo gönderim tarihi
    /// </summary>
    public DateTime? ShippedDate { get; set; }

    /// <summary>
    /// Kargo teslim tarihi
    /// </summary>
    public DateTime? DeliveredDate { get; set; }

    /// <summary>
    /// Kargo tahmini teslim tarihi
    /// </summary>
    public DateTime? EstimatedDeliveryDate { get; set; }

    /// <summary>
    /// Kargo gönderen adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string SenderName { get; set; } = string.Empty;

    /// <summary>
    /// Kargo gönderen telefonu
    /// </summary>
    [MaxLength(20)]
    public string? SenderPhone { get; set; }

    /// <summary>
    /// Kargo gönderen adresi
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string SenderAddress { get; set; } = string.Empty;

    /// <summary>
    /// Kargo alıcı adı
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string ReceiverName { get; set; } = string.Empty;

    /// <summary>
    /// Kargo alıcı telefonu
    /// </summary>
    [MaxLength(20)]
    public string? ReceiverPhone { get; set; }

    /// <summary>
    /// Kargo alıcı adresi
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string ReceiverAddress { get; set; } = string.Empty;

    /// <summary>
    /// Kargo notları
    /// </summary>
    [MaxLength(1000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Kargo şirketi referans numarası
    /// </summary>
    [MaxLength(100)]
    public string? CompanyReferenceNumber { get; set; }

    /// <summary>
    /// Kargo takip URL'si
    /// </summary>
    [MaxLength(300)]
    public string? TrackingUrl { get; set; }

    /// <summary>
    /// Kargo sigorta tutarı
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal? InsuranceAmount { get; set; }

    /// <summary>
    /// Kargo özel talimatları
    /// </summary>
    [MaxLength(500)]
    public string? SpecialInstructions { get; set; }

    /// <summary>
    /// Kargo ile ilgili sipariş
    /// </summary>
    public virtual Order Order { get; set; } = null!;

    /// <summary>
    /// Kargo şirketi
    /// </summary>
    public virtual CargoCompany CargoCompany { get; set; } = null!;

    /// <summary>
    /// Kargo takip geçmişi
    /// </summary>
    public virtual ICollection<CargoTracking> TrackingHistory { get; set; } = new List<CargoTracking>();
}
