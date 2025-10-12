namespace ECommerce.Domain.Enums;

/// <summary>
/// Ödeme yöntemleri
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Kredi kartı
    /// </summary>
    CreditCard = 0,

    /// <summary>
    /// Banka kartı
    /// </summary>
    DebitCard = 1,

    /// <summary>
    /// Kapıda ödeme
    /// </summary>
    CashOnDelivery = 2,

    /// <summary>
    /// Banka havalesi
    /// </summary>
    BankTransfer = 3,

    /// <summary>
    /// Dijital cüzdan
    /// </summary>
    DigitalWallet = 4,

    /// <summary>
    /// Taksitli ödeme
    /// </summary>
    Installment = 5
}
