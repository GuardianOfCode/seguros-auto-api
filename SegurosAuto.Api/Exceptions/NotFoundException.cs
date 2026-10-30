namespace SegurosAuto.Api.Exceptions;

/// <summary>Recurso inexistente → HTTP 404 Not Found.</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}