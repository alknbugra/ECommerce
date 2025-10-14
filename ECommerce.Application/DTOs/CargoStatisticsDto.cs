using ECommerce.Domain.Enums;

namespace ECommerce.Application.DTOs;

/// <summary>
/// Kargo istatistikleri DTO'su
/// </summary>
public class CargoStatisticsDto
{
    /// <summary>
    /// Toplam kargo sayısı
    /// </summary>
    public int TotalCargos { get; set; }

    /// <summary>
    /// Duruma göre kargo sayıları
    /// </summary>
    public Dictionary<CargoStatus, int> StatusCounts { get; set; } = new();

    /// <summary>
    /// Kargo şirketine göre kargo sayıları
    /// </summary>
    public Dictionary<string, int> CompanyCounts { get; set; } = new();

    /// <summary>
    /// Toplam kargo ücreti
    /// </summary>
    public decimal TotalShippingCost { get; set; }

    /// <summary>
    /// Ortalama kargo süresi (gün)
    /// </summary>
    public double AverageDeliveryDays { get; set; }
}
