using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs;

/// <summary>
/// Kargo DTO'su
/// </summary>
public class CargoDto
{
    /// <summary>
    /// Kargo ID'si
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kargo takip numarası
    /// </summary>
    public string TrackingNumber { get; set; } = string.Empty;

    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Sipariş numarası
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi ID'si
    /// </summary>
    public Guid CargoCompanyId { get; set; }

    /// <summary>
    /// Kargo şirketi adı
    /// </summary>
    public string CargoCompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Kargo şirketi kodu
    /// </summary>
    public string CargoCompanyCode { get; set; } = string.Empty;

    /// <summary>
    /// Kargo durumu
    /// </summary>
    public CargoStatus Status { get; set; }

    /// <summary>
    /// Kargo durumu açıklaması
    /// </summary>
    public string StatusDescription { get; set; } = string.Empty;

    /// <summary>
    /// Kargo türü
    /// </summary>
    public CargoType Type { get; set; }

    /// <summary>
    /// Kargo türü açıklaması
    /// </summary>
    public string TypeDescription { get; set; } = string.Empty;

    /// <summary>
    /// Kargo ağırlığı (kg)
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Kargo boyutları
    /// </summary>
    public string? Dimensions { get; set; }

    /// <summary>
    /// Kargo ücreti
    /// </summary>
    public decimal ShippingCost { get; set; }

    /// <summary>
    /// Kargo ücreti (müşteriye yansıtılan)
    /// </summary>
    public decimal CustomerShippingCost { get; set; }

    /// <summary>
    /// Kargo içeriği açıklaması
    /// </summary>
    public string? ContentDescription { get; set; }

    /// <summary>
    /// Kargo değeri
    /// </summary>
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
    /// Kargo gönderen bilgileri
    /// </summary>
    public CargoSenderDto Sender { get; set; } = new();

    /// <summary>
    /// Kargo alıcı bilgileri
    /// </summary>
    public CargoReceiverDto Receiver { get; set; } = new();

    /// <summary>
    /// Kargo notları
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Kargo şirketi referans numarası
    /// </summary>
    public string? CompanyReferenceNumber { get; set; }

    /// <summary>
    /// Kargo takip URL'si
    /// </summary>
    public string? TrackingUrl { get; set; }

    /// <summary>
    /// Kargo sigorta tutarı
    /// </summary>
    public decimal? InsuranceAmount { get; set; }

    /// <summary>
    /// Kargo özel talimatları
    /// </summary>
    public string? SpecialInstructions { get; set; }

    /// <summary>
    /// Kargo oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Kargo güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Kargo takip geçmişi
    /// </summary>
    public List<CargoTrackingDto> TrackingHistory { get; set; } = new();
}

/// <summary>
/// Kargo gönderen bilgileri DTO'su
/// </summary>
public class CargoSenderDto
{
    /// <summary>
    /// Gönderen adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gönderen telefonu
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Gönderen adresi
    /// </summary>
    public string Address { get; set; } = string.Empty;
}

/// <summary>
/// Kargo alıcı bilgileri DTO'su
/// </summary>
public class CargoReceiverDto
{
    /// <summary>
    /// Alıcı adı
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Alıcı telefonu
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Alıcı adresi
    /// </summary>
    public string Address { get; set; } = string.Empty;
}
