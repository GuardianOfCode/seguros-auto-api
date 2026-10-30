using System.ComponentModel.DataAnnotations;

namespace SegurosAuto.Api.Dtos;

/// <summary>Datos para crear (POST) o actualizar (PUT) una cobertura del catálogo.</summary>
public class GuardarCoberturaRequest
{
    /// <summary>Nombre de la cobertura.</summary>
    /// <example>Cristales</example>
    [Required, StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Monto de la cobertura (mayor a cero). Se suma a la prima total al emitir.</summary>
    /// <example>400.00</example>
    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "El monto de la cobertura debe ser mayor a cero.")]
    public decimal MontoCobertura { get; set; }
}

/// <summary>Cobertura devuelta por la API.</summary>
public class CoberturaResponse
{
    /// <summary>Identificador de la cobertura.</summary>
    /// <example>1</example>
    public int Id { get; set; }

    /// <summary>Nombre de la cobertura.</summary>
    /// <example>Responsabilidad Civil</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Monto de la cobertura.</summary>
    /// <example>1500.00</example>
    public decimal MontoCobertura { get; set; }
}