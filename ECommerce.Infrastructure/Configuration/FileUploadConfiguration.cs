namespace ECommerce.Infrastructure.Configuration;

/// <summary>
/// Dosya yükleme ayarları
/// </summary>
public class FileUploadConfiguration
{
    /// <summary>
    /// Dosya yükleme ana klasörü
    /// </summary>
    public string UploadPath { get; set; } = "wwwroot/uploads";

    /// <summary>
    /// Web URL base path
    /// </summary>
    public string WebBasePath { get; set; } = "/uploads";

    /// <summary>
    /// Maksimum dosya boyutu (byte)
    /// </summary>
    public long MaxFileSize { get; set; } = 5 * 1024 * 1024; // 5MB

    /// <summary>
    /// Desteklenen dosya tipleri
    /// </summary>
    public List<string> AllowedFileTypes { get; set; } = new()
    {
        "image/jpeg",
        "image/jpg", 
        "image/png",
        "image/gif",
        "image/webp"
    };

    /// <summary>
    /// Desteklenen dosya uzantıları
    /// </summary>
    public List<string> AllowedExtensions { get; set; } = new()
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".gif",
        ".webp"
    };

    /// <summary>
    /// Dosya adı için geçerli karakterler
    /// </summary>
    public string ValidFileNamePattern { get; set; } = @"^[a-zA-Z0-9._-]+$";

    /// <summary>
    /// Dosya adı maksimum uzunluğu
    /// </summary>
    public int MaxFileNameLength { get; set; } = 100;
}
