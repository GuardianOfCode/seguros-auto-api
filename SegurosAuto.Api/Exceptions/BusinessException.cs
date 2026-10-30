namespace SegurosAuto.Api.Exceptions;

/// <summary>Regla de negocio incumplida → HTTP 400 Bad Request.</summary>
public class BusinessException : Exception
{
    public BusinessException(string message) : base(message) { }
}
