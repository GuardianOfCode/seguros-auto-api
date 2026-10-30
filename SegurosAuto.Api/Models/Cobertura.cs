namespace SegurosAuto.Api.Models;

/// <summary>
/// Tipo de cobertura disponible en el catálogo (Robo, Choque, Responsabilidad Civil...).
/// </summary>
public class Cobertura
{
    /// <summary>Identificador único (llave primaria, autoincremental).</summary>
    public int Id { get; set; }

    /// <summary>Nombre de la cobertura.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Monto de la cobertura; es lo que se suma a la prima total al emitir.</summary>
    public decimal MontoCobertura { get; set; }

    /// <summary>Relación con las pólizas que incluyen esta cobertura.</summary>
    public ICollection<PolizaCobertura> PolizaCoberturas { get; set; } = new List<PolizaCobertura>();
}