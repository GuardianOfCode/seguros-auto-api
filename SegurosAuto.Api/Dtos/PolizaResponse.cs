namespace SegurosAuto.Api.Dtos;

/// <summary>Vehículo asegurado devuelto por la API.</summary>
public class VehiculoResponse
{
    /// <summary>Identificador del vehículo.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Placa.</summary>
    /// <example>M123456</example>
    public string Placa { get; set; } = string.Empty;

    /// <summary>Marca.</summary>
    /// <example>Toyota</example>
    public string Marca { get; set; } = string.Empty;

    /// <summary>Modelo.</summary>
    /// <example>Corolla</example>
    public string Modelo { get; set; } = string.Empty;

    /// <summary>Año de fabricación.</summary>
    /// <example>2022</example>
    public int Anio { get; set; }

    /// <summary>Valor comercial.</summary>
    /// <example>18500.00</example>
    public decimal ValorComercial { get; set; }
}

/// <summary>Detalle completo de una póliza emitida.</summary>
public class PolizaResponse
{
    /// <summary>Identificador de la póliza.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Número de póliza autogenerado.</summary>
    /// <example>POL-2026-000001</example>
    public string NumeroPoliza { get; set; } = string.Empty;

    /// <summary>Fecha y hora de emisión (UTC).</summary>
    /// <example>2026-10-30T15:30:00Z</example>
    public DateTime FechaEmision { get; set; }

    /// <summary>Suma asegurada (valor comercial del vehículo).</summary>
    /// <example>18500.00</example>
    public decimal SumaAsegurada { get; set; }

    /// <summary>Prima total a pagar (suma de los montos de las coberturas).</summary>
    /// <example>6000.00</example>
    public decimal PrimaTotal { get; set; }

    /// <summary>Cliente titular.</summary>
    public ClienteResponse Cliente { get; set; } = null!;

    /// <summary>Vehículo asegurado.</summary>
    public VehiculoResponse Vehiculo { get; set; } = null!;

    /// <summary>Coberturas contratadas (con el monto vigente al emitir).</summary>
    public List<CoberturaResponse> Coberturas { get; set; } = new();
}