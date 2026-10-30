using System.ComponentModel.DataAnnotations;

namespace SegurosAuto.Api.Dtos;

/// <summary>Datos del vehículo que se va a asegurar.</summary>
public class VehiculoRequest
{
    /// <summary>Placa del vehículo. Si ya existe en el sistema, se reutiliza y se actualizan sus datos.</summary>
    /// <example>M123456</example>
    [Required, StringLength(15)]
    public string Placa { get; set; } = string.Empty;

    /// <summary>Marca del vehículo.</summary>
    /// <example>Toyota</example>
    [Required, StringLength(50)]
    public string Marca { get; set; } = string.Empty;

    /// <summary>Modelo del vehículo.</summary>
    /// <example>Corolla</example>
    [Required, StringLength(50)]
    public string Modelo { get; set; } = string.Empty;

    /// <summary>Año de fabricación (entre 1980 y el año actual + 1).</summary>
    /// <example>2022</example>
    [Range(1980, 2100, ErrorMessage = "El año del vehículo no es válido.")]
    public int Anio { get; set; }

    /// <summary>Valor comercial del vehículo (mayor a cero). Se usa como suma asegurada.</summary>
    /// <example>18500.00</example>
    [Range(typeof(decimal), "1", "999999999", ErrorMessage = "El valor comercial debe ser mayor a cero.")]
    public decimal ValorComercial { get; set; }
}

/// <summary>Solicitud para emitir una póliza de auto.</summary>
public class EmitirPolizaRequest
{
    /// <summary>Id del cliente titular (debe existir).</summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "ClienteId es obligatorio.")]
    public int ClienteId { get; set; }

    /// <summary>Datos del vehículo a asegurar.</summary>
    [Required]
    public VehiculoRequest Vehiculo { get; set; } = null!;

    /// <summary>Ids de las coberturas seleccionadas (al menos una; todas deben existir).</summary>
    /// <example>[1, 2, 3]</example>
    [Required, MinLength(1, ErrorMessage = "Debe seleccionar al menos una cobertura.")]
    public List<int> CoberturaIds { get; set; } = new();
}