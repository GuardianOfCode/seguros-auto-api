namespace SegurosAuto.Api.Models;

/// <summary>
/// Póliza de seguro emitida. Una vez emitida no se modifica ni se elimina.
/// </summary>
public class Poliza
{
    /// <summary>Identificador único (llave primaria, autoincremental).</summary>
    public int Id { get; set; }

    /// <summary>Número de póliza autogenerado por el sistema. Formato: POL-2026-000001.</summary>
    public string NumeroPoliza { get; set; } = string.Empty;

    /// <summary>Llave foránea hacia <see cref="Cliente"/>.</summary>
    public int ClienteId { get; set; }

    /// <summary>Llave foránea hacia <see cref="Vehiculo"/>.</summary>
    public int VehiculoId { get; set; }

    /// <summary>Fecha y hora de emisión (UTC).</summary>
    public DateTime FechaEmision { get; set; }

    /// <summary>Monto asegurado (valor comercial del vehículo).</summary>
    public decimal SumaAsegurada { get; set; }

    /// <summary>Monto total a pagar, calculado en el servidor (suma de las coberturas).</summary>
    public decimal PrimaTotal { get; set; }

    /// <summary>Cliente titular de la póliza.</summary>
    public Cliente Cliente { get; set; } = null!;

    /// <summary>Vehículo asegurado.</summary>
    public Vehiculo Vehiculo { get; set; } = null!;

    /// <summary>Coberturas contratadas en esta póliza.</summary>
    public ICollection<PolizaCobertura> Coberturas { get; set; } = new List<PolizaCobertura>();
}