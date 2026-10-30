namespace SegurosAuto.Api.Models;

/// <summary>
/// Persona (titular) a la que se le emite una póliza.
/// </summary>
public class Cliente
{
    /// <summary>Identificador único (llave primaria, autoincremental).</summary>
    public int Id { get; set; }

    /// <summary>Nombre completo del cliente.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Documento de identidad (DNI / RUC / cédula). Es único en la base de datos.</summary>
    public string Identificacion { get; set; } = string.Empty;

    /// <summary>Correo electrónico de contacto.</summary>
    public string Correo { get; set; } = string.Empty;

    /// <summary>Pólizas emitidas a este cliente.</summary>
    public ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
}