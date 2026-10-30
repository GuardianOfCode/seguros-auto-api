namespace SegurosAuto.Api.Models;

/// <summary>
/// Tabla intermedia (muchos a muchos) entre Póliza y Cobertura.
/// Guarda el monto vigente al momento de emitir, así la póliza no cambia
/// si luego se modifica el catálogo de coberturas.
/// </summary>
public class PolizaCobertura
{
    /// <summary>Llave foránea hacia la póliza (parte de la llave compuesta).</summary>
    public int PolizaId { get; set; }

    /// <summary>Llave foránea hacia la cobertura (parte de la llave compuesta).</summary>
    public int CoberturaId { get; set; }

    /// <summary>Monto de la cobertura vigente al momento de emitir la póliza.</summary>
    public decimal MontoCobertura { get; set; }

    /// <summary>Póliza a la que pertenece.</summary>
    public Poliza Poliza { get; set; } = null!;

    /// <summary>Cobertura contratada.</summary>
    public Cobertura Cobertura { get; set; } = null!;
}