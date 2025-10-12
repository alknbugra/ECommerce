using Microsoft.AspNetCore.Http;

namespace ECommerce.Application.Common.Interfaces;

/// <summary>
/// Dosya yükleme servisi arayüzü
/// </summary>
public interface IFileUploadService
{
    /// <summary>
    /// Dosya yükler
    /// </summary>
    /// <param name="file">Yüklenecek dosya</param>
    /// <param name="folder">Dosya klasörü (örn: products, categories, users)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Yüklenen dosya bilgileri</returns>
    Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Birden fazla dosya yükler
    /// </summary>
    /// <param name="files">Yüklenecek dosyalar</param>
    /// <param name="folder">Dosya klasörü</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Yüklenen dosya bilgileri listesi</returns>
    Task<List<FileUploadResult>> UploadFilesAsync(IFormFileCollection files, string folder, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dosyayı siler
    /// </summary>
    /// <param name="filePath">Silinecek dosya yolu</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Silme işlemi başarılı mı?</returns>
    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Dosya var mı kontrol eder
    /// </summary>
    /// <param name="filePath">Kontrol edilecek dosya yolu</param>
    /// <returns>Dosya var mı?</returns>
    bool FileExists(string filePath);

    /// <summary>
    /// Dosya boyutunu kontrol eder
    /// </summary>
    /// <param name="file">Kontrol edilecek dosya</param>
    /// <returns>Dosya boyutu geçerli mi?</returns>
    bool IsValidFileSize(IFormFile file);

    /// <summary>
    /// Dosya tipini kontrol eder
    /// </summary>
    /// <param name="file">Kontrol edilecek dosya</param>
    /// <returns>Dosya tipi geçerli mi?</returns>
    bool IsValidFileType(IFormFile file);

    /// <summary>
    /// Desteklenen dosya tiplerini döndürür
    /// </summary>
    /// <returns>Desteklenen dosya tipleri</returns>
    IEnumerable<string> GetSupportedFileTypes();

    /// <summary>
    /// Maksimum dosya boyutunu döndürür (byte)
    /// </summary>
    /// <returns>Maksimum dosya boyutu</returns>
    long GetMaxFileSize();
}

/// <summary>
/// Dosya yükleme sonucu
/// </summary>
public class FileUploadResult
{
    /// <summary>
    /// Yükleme başarılı mı?
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Dosya yolu (web'den erişilebilir)
    /// </summary>
    public string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Fiziksel dosya yolu
    /// </summary>
    public string PhysicalPath { get; set; } = string.Empty;

    /// <summary>
    /// Dosya adı
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Dosya boyutu (byte)
    /// </summary>
    public long FileSize { get; set; }

    /// <summary>
    /// Dosya tipi (MIME type)
    /// </summary>
    public string ContentType { get; set; } = string.Empty;

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string? ErrorMessage { get; set; }
}
