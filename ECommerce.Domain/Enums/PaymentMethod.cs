namespace ECommerce.Domain.Enums;

/// <summary>
/// Ödeme yöntemi enum'u
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
    /// Havale/EFT
    /// </summary>
    BankTransfer = 2,

    /// <summary>
    /// Kapıda ödeme
    /// </summary>
    CashOnDelivery = 3,

    /// <summary>
    /// Dijital cüzdan
    /// </summary>
    DigitalWallet = 4,

    /// <summary>
    /// Taksitli ödeme
    /// </summary>
    Installment = 5
}