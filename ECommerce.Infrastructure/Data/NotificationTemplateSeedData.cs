using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;

/// <summary>
/// Bildirim şablonu seed data'sı
/// </summary>
public static class NotificationTemplateSeedData
{
    /// <summary>
    /// Varsayılan bildirim şablonlarını ekle
    /// </summary>
    public static void SeedNotificationTemplates(ModelBuilder modelBuilder)
    {
        var templates = new List<NotificationTemplate>
        {
            // Sipariş durumu bildirimleri
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sipariş Onaylandı",
                Code = "ORDER_CONFIRMED",
                Type = NotificationType.Order,
                TitleTemplate = "Siparişiniz Onaylandı - {OrderNumber}",
                ContentTemplate = "Merhaba! {OrderNumber} numaralı siparişiniz başarıyla onaylandı. Toplam tutar: {TotalAmount} TL. Siparişiniz hazırlanmaya başlanacaktır.",
                DefaultPriority = NotificationPriority.High,
                IsActive = true,
                Description = "Sipariş onaylandığında gönderilen bildirim şablonu",
                Variables = "OrderNumber, TotalAmount, ItemCount",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sipariş Hazırlanıyor",
                Code = "ORDER_PREPARING",
                Type = NotificationType.Order,
                TitleTemplate = "Siparişiniz Hazırlanıyor - {OrderNumber}",
                ContentTemplate = "{OrderNumber} numaralı siparişiniz hazırlanmaya başlandı. {ItemCount} ürün paketleniyor.",
                DefaultPriority = NotificationPriority.Normal,
                IsActive = true,
                Description = "Sipariş hazırlanmaya başladığında gönderilen bildirim şablonu",
                Variables = "OrderNumber, ItemCount",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sipariş Kargoya Verildi",
                Code = "ORDER_SHIPPED",
                Type = NotificationType.Shipping,
                TitleTemplate = "Siparişiniz Kargoya Verildi - {OrderNumber}",
                ContentTemplate = "{OrderNumber} numaralı siparişiniz kargoya verildi. Kargo takip numarası: {TrackingNumber}. {ShippingCompany} ile gönderildi.",
                DefaultPriority = NotificationPriority.High,
                IsActive = true,
                Description = "Sipariş kargoya verildiğinde gönderilen bildirim şablonu",
                Variables = "OrderNumber, TrackingNumber, ShippingCompany",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sipariş Teslim Edildi",
                Code = "ORDER_DELIVERED",
                Type = NotificationType.Order,
                TitleTemplate = "Siparişiniz Teslim Edildi - {OrderNumber}",
                ContentTemplate = "{OrderNumber} numaralı siparişiniz başarıyla teslim edildi. Ürünlerinizi beğendiyseniz değerlendirme yapabilirsiniz.",
                DefaultPriority = NotificationPriority.Normal,
                IsActive = true,
                Description = "Sipariş teslim edildiğinde gönderilen bildirim şablonu",
                Variables = "OrderNumber",
                CreatedAt = DateTime.UtcNow
            },

            // Stok bildirimleri
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Stok Uyarısı",
                Code = "STOCK_ALERT",
                Type = NotificationType.Stock,
                TitleTemplate = "Stok Uyarısı - {ProductName}",
                ContentTemplate = "{ProductName} ürününün stok miktarı azaldı. Mevcut stok: {CurrentStock} adet. Hemen sipariş verin!",
                DefaultPriority = NotificationPriority.High,
                IsActive = true,
                Description = "Ürün stok miktarı azaldığında gönderilen bildirim şablonu",
                Variables = "ProductName, CurrentStock, ProductId",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Stok Tükendi",
                Code = "STOCK_OUT",
                Type = NotificationType.Stock,
                TitleTemplate = "Stok Tükendi - {ProductName}",
                ContentTemplate = "{ProductName} ürününün stoku tükendi. Stok geldiğinde size bildirim göndereceğiz.",
                DefaultPriority = NotificationPriority.Critical,
                IsActive = true,
                Description = "Ürün stoku tükendiğinde gönderilen bildirim şablonu",
                Variables = "ProductName, ProductId",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Stok Geldi",
                Code = "STOCK_IN",
                Type = NotificationType.Stock,
                TitleTemplate = "Stok Geldi - {ProductName}",
                ContentTemplate = "{ProductName} ürününün stoku geldi! Hemen sipariş verin.",
                DefaultPriority = NotificationPriority.High,
                IsActive = true,
                Description = "Ürün stoku geldiğinde gönderilen bildirim şablonu",
                Variables = "ProductName, ProductId",
                CreatedAt = DateTime.UtcNow
            },

            // Fiyat bildirimleri
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Fiyat Düştü",
                Code = "PRICE_DROP",
                Type = NotificationType.Price,
                TitleTemplate = "Fiyat Düştü! - {ProductName}",
                ContentTemplate = "{ProductName} ürününün fiyatı {OldPrice} TL'den {NewPrice} TL'ye düştü. İndirim oranı: %{PriceChangePercent:F1}",
                DefaultPriority = NotificationPriority.High,
                IsActive = true,
                Description = "Ürün fiyatı düştüğünde gönderilen bildirim şablonu",
                Variables = "ProductName, OldPrice, NewPrice, PriceChange, PriceChangePercent, ProductId",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Fiyat Arttı",
                Code = "PRICE_INCREASE",
                Type = NotificationType.Price,
                TitleTemplate = "Fiyat Güncellendi - {ProductName}",
                ContentTemplate = "{ProductName} ürününün fiyatı {OldPrice} TL'den {NewPrice} TL'ye güncellendi.",
                DefaultPriority = NotificationPriority.Normal,
                IsActive = true,
                Description = "Ürün fiyatı arttığında gönderilen bildirim şablonu",
                Variables = "ProductName, OldPrice, NewPrice, PriceChange, ProductId",
                CreatedAt = DateTime.UtcNow
            },

            // Ödeme bildirimleri
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Ödeme Başarılı",
                Code = "PAYMENT_SUCCESS",
                Type = NotificationType.Payment,
                TitleTemplate = "Ödeme Başarılı - {OrderNumber}",
                ContentTemplate = "{OrderNumber} numaralı siparişinizin ödemesi başarıyla alındı. Tutar: {Amount} TL",
                DefaultPriority = NotificationPriority.High,
                IsActive = true,
                Description = "Ödeme başarılı olduğunda gönderilen bildirim şablonu",
                Variables = "OrderNumber, Amount",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Ödeme Başarısız",
                Code = "PAYMENT_FAILED",
                Type = NotificationType.Payment,
                TitleTemplate = "Ödeme Başarısız - {OrderNumber}",
                ContentTemplate = "{OrderNumber} numaralı siparişinizin ödemesi alınamadı. Lütfen ödeme bilgilerinizi kontrol edin.",
                DefaultPriority = NotificationPriority.Critical,
                IsActive = true,
                Description = "Ödeme başarısız olduğunda gönderilen bildirim şablonu",
                Variables = "OrderNumber, ErrorMessage",
                CreatedAt = DateTime.UtcNow
            },

            // Sistem bildirimleri
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Hoş Geldiniz",
                Code = "WELCOME",
                Type = NotificationType.System,
                TitleTemplate = "Hoş Geldiniz!",
                ContentTemplate = "E-ticaret sitemize hoş geldiniz! İlk siparişinizde %10 indirim kazanın.",
                DefaultPriority = NotificationPriority.Normal,
                IsActive = true,
                Description = "Yeni kullanıcı kaydı olduğunda gönderilen hoş geldin bildirimi",
                Variables = "",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Kampanya Bildirimi",
                Code = "CAMPAIGN",
                Type = NotificationType.Campaign,
                TitleTemplate = "Özel Kampanya - {CampaignName}",
                ContentTemplate = "{CampaignName} kampanyası başladı! {DiscountPercent}% indirim fırsatını kaçırmayın. Son tarih: {EndDate}",
                DefaultPriority = NotificationPriority.High,
                IsActive = true,
                Description = "Kampanya duyurusu için bildirim şablonu",
                Variables = "CampaignName, DiscountPercent, EndDate",
                CreatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<NotificationTemplate>().HasData(templates);
    }
}
