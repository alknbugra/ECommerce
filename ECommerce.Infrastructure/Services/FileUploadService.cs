using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace ECommerce.Infrastructure.Services;

/// <summary>
/// Dosya yükleme servisi implementasyonu
/// </summary>
public class FileUploadService : IFileUploadService
{
    private readonly FileUploadConfiguration _config;
    private readonly ILogger<FileUploadService> _logger;
    private readonly string _uploadPath;

    public FileUploadService(
        IOptions<FileUploadConfiguration> config,
        ILogger<FileUploadService> logger)
    {
        _config = config.Value;
        _logger = logger;
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), _config.UploadPath);
    }

    public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Dosya yükleniyor: {FileName}, Boyut: {FileSize}, Tip: {ContentType}", 
                file.FileName, file.Length, file.ContentType);

            // Dosya validasyonu
            if (!IsValidFileType(file))
            {
                return new FileUploadResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Desteklenmeyen dosya tipi: {file.ContentType}. Desteklenen tipler: {string.Join(", ", _config.AllowedFileTypes)}"
                };
            }

            if (!IsValidFileSize(file))
            {
                return new FileUploadResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Dosya boyutu çok büyük: {file.Length} byte. Maksimum boyut: {_config.MaxFileSize} byte"
                };
            }

            // Dosya adını güvenli hale getir
            var safeFileName = GenerateSafeFileName(file.FileName);
            if (string.IsNullOrEmpty(safeFileName))
            {
                return new FileUploadResult
                {
                    IsSuccess = false,
                    ErrorMessage = "Geçersiz dosya adı"
                };
            }

            // Klasör yapısını oluştur
            var folderPath = Path.Combine(_uploadPath, folder);
            Directory.CreateDirectory(folderPath);

            // Benzersiz dosya adı oluştur
            var uniqueFileName = GenerateUniqueFileName(safeFileName);
            var filePath = Path.Combine(folderPath, uniqueFileName);
            var webPath = $"{_config.WebBasePath}/{folder}/{uniqueFileName}";

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, cancellationToken);
            }

            _logger.LogInformation("Dosya başarıyla yüklendi: {FilePath}", webPath);

            return new FileUploadResult
            {
                IsSuccess = true,
                FilePath = webPath,
                PhysicalPath = filePath,
                FileName = uniqueFileName,
                FileSize = file.Length,
                ContentType = file.ContentType
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dosya yükleme hatası: {FileName}", file.FileName);
            return new FileUploadResult
            {
                IsSuccess = false,
                ErrorMessage = $"Dosya yükleme hatası: {ex.Message}"
            };
        }
    }

    public async Task<List<FileUploadResult>> UploadFilesAsync(IFormFileCollection files, string folder, CancellationToken cancellationToken = default)
    {
        var results = new List<FileUploadResult>();

        foreach (var file in files)
        {
            var result = await UploadFileAsync(file, folder, cancellationToken);
            results.Add(result);
        }

        return results;
    }

    public async Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            // Web path'ini fiziksel path'e çevir
            var physicalPath = ConvertWebPathToPhysicalPath(filePath);
            
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
                _logger.LogInformation("Dosya silindi: {FilePath}", filePath);
                return true;
            }

            _logger.LogWarning("Silinecek dosya bulunamadı: {FilePath}", filePath);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Dosya silme hatası: {FilePath}", filePath);
            return false;
        }
    }

    public bool FileExists(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return false;
        }

        var physicalPath = ConvertWebPathToPhysicalPath(filePath);
        return File.Exists(physicalPath);
    }

    public bool IsValidFileSize(IFormFile file)
    {
        return file.Length > 0 && file.Length <= _config.MaxFileSize;
    }

    public bool IsValidFileType(IFormFile file)
    {
        if (string.IsNullOrEmpty(file.ContentType))
        {
            return false;
        }

        return _config.AllowedFileTypes.Contains(file.ContentType.ToLower());
    }

    public IEnumerable<string> GetSupportedFileTypes()
    {
        return _config.AllowedFileTypes;
    }

    public long GetMaxFileSize()
    {
        return _config.MaxFileSize;
    }

    /// <summary>
    /// Güvenli dosya adı oluştur
    /// </summary>
    private string GenerateSafeFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            return string.Empty;
        }

        // Dosya adından uzantıyı ayır
        var extension = Path.GetExtension(fileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

        // Geçersiz karakterleri temizle
        var cleanName = Regex.Replace(nameWithoutExtension, @"[^a-zA-Z0-9._-]", "_");
        
        // Uzunluk kontrolü
        if (cleanName.Length > _config.MaxFileNameLength - extension.Length)
        {
            cleanName = cleanName.Substring(0, _config.MaxFileNameLength - extension.Length);
        }

        // Boş ise varsayılan ad ver
        if (string.IsNullOrEmpty(cleanName))
        {
            cleanName = "file";
        }

        return cleanName + extension;
    }

    /// <summary>
    /// Benzersiz dosya adı oluştur
    /// </summary>
    private string GenerateUniqueFileName(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        
        return $"{nameWithoutExtension}_{timestamp}_{random}{extension}";
    }

    /// <summary>
    /// Web path'ini fiziksel path'e çevir
    /// </summary>
    private string ConvertWebPathToPhysicalPath(string webPath)
    {
        if (webPath.StartsWith(_config.WebBasePath))
        {
            var relativePath = webPath.Substring(_config.WebBasePath.Length).TrimStart('/');
            return Path.Combine(_uploadPath, relativePath);
        }

        return Path.Combine(_uploadPath, webPath.TrimStart('/'));
    }
}
