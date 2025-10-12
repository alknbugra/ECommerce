using FluentValidation.Results;

namespace ECommerce.Application.Common.Exceptions;

/// <summary>
/// Validation exception sınıfı
/// </summary>
public class ValidationException : ApplicationException
{
    /// <summary>
    /// Validation hataları
    /// </summary>
    public List<ValidationError> Errors { get; }

    public ValidationException() : base("Bir veya daha fazla validation hatası oluştu.", "VALIDATION_ERROR")
    {
        Errors = new List<ValidationError>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures.Select(f => new ValidationError
        {
            PropertyName = f.PropertyName,
            ErrorMessage = f.ErrorMessage,
            AttemptedValue = f.AttemptedValue
        }).ToList();
    }

    public ValidationException(string message) : base(message, "VALIDATION_ERROR")
    {
        Errors = new List<ValidationError>();
    }

    public ValidationException(string message, IEnumerable<ValidationFailure> failures) : base(message, "VALIDATION_ERROR")
    {
        Errors = failures.Select(f => new ValidationError
        {
            PropertyName = f.PropertyName,
            ErrorMessage = f.ErrorMessage,
            AttemptedValue = f.AttemptedValue
        }).ToList();
    }
}

/// <summary>
/// Validation hata modeli
/// </summary>
public class ValidationError
{
    /// <summary>
    /// Özellik adı
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    /// <summary>
    /// Hata mesajı
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Denenen değer
    /// </summary>
    public object? AttemptedValue { get; set; }
}
