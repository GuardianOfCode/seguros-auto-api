namespace SegurosAuto.Api.Models;

/// <summary>
/// Automóvil que será asegurado.
/// </summary>
public class Vehiculo
{
    /// <summary>Identificador único (llave primaria, autoincremental).</summary>
    public int Id { get; set; }

    /// <summary>Placa del vehículo. Es única en la base de datos.</summary>
    public string Placa { get; set; } = string.Empty;

    /// <summary>Marca (Toyota, Kia, etc.).</summary>
    public string Marca { get; set; } = string.Empty;

    /// <summary>Modelo (Corolla, Rio, etc.).</summary>
    public string Modelo { get; set; } = string.Empty;

    /// <summary>Año de fabricación.</summary>
    public int Anio { get; set; }

    /// <summary>Valor comercial del vehículo; se usa como suma asegurada.</summary>
    public decimal ValorComercial { get; set; }

    /// <summary>Pólizas emitidas para este vehículo.</summary>
    public ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
}