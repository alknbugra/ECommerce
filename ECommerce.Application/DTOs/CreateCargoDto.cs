using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs;

/// <summary>
/// Kargo oluşturma DTO'su
/// </summary>
public class CreateCargoDto
{
    /// <summary>
    /// Sipariş ID'si
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Kargo şirketi ID'si
    /// </summary>
    public Guid CargoCompanyId { get; set; }

    /// <summary>
    /// Kargo türü
    /// </summary>
    public CargoType Type { get; set; } = CargoType.Standard;

    /// <summary>
    /// Kargo ağırlığı (kg)
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Kargo boyutları (cm) - Uzunluk x Genişlik x Yükseklik
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
    /// Kargo sigorta tutarı
    /// </summary>
    public decimal? InsuranceAmount { get; set; }

    /// <summary>
    /// Kargo özel talimatları
    /// </summary>
    public string? SpecialInstructions { get; set; }
}

/// <summary>
/// Kargo güncelleme DTO'su
/// </summary>
public class UpdateCargoDto
{
    /// <summary>
    /// Kargo şirketi ID'si
    /// </summary>
    public Guid CargoCompanyId { get; set; }

    /// <summary>
    /// Kargo durumu
    /// </summary>
    public CargoStatus Status { get; set; }

    /// <summary>
    /// Kargo türü
    /// </summary>
    public CargoType Type { get; set; }

    /// <summary>
    /// Kargo ağırlığı (kg)
    /// </summary>
    public decimal Weight { get; set; }

    /// <summary>
    /// Kargo boyutları (cm) - Uzunluk x Genişlik x Yükseklik
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
}
