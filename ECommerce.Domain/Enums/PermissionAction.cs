namespace ECommerce.Domain.Enums;

/// <summary>
/// Yetki eylemleri
/// </summary>
public enum PermissionAction
{
    /// <summary>
    /// Okuma yetkisi
    /// </summary>
    Read = 1,

    /// <summary>
    /// Oluşturma yetkisi
    /// </summary>
    Create = 2,

    /// <summary>
    /// Güncelleme yetkisi
    /// </summary>
    Update = 3,

    /// <summary>
    /// Silme yetkisi
    /// </summary>
    Delete = 4,

    /// <summary>
    /// Tam yönetim yetkisi
    /// </summary>
    Manage = 5,

    /// <summary>
    /// Onaylama yetkisi
    /// </summary>
    Approve = 6,

    /// <summary>
    /// Reddetme yetkisi
    /// </summary>
    Reject = 7,

    /// <summary>
    /// Dışa aktarma yetkisi
    /// </summary>
    Export = 8,

    /// <summary>
    /// İçe aktarma yetkisi
    /// </summary>
    Import = 9,

    /// <summary>
    /// Yapılandırma yetkisi
    /// </summary>
    Configure = 10
}
