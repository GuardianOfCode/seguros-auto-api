using Microsoft.AspNetCore.Mvc;
using SegurosAuto.Api.Dtos;
using SegurosAuto.Api.Services;

namespace SegurosAuto.Api.Controllers;

/// <summary>
/// Emisión y consulta de pólizas de auto. Una póliza emitida es inmutable:
/// solo se crea, se consulta y se lista (no se edita ni se elimina).
/// </summary>
[ApiController]
[Route("api/polizas")]
[Produces("application/json")]
public class PolizasController : ControllerBase
{
    private readonly IPolizaService _service;
    public PolizasController(IPolizaService service) => _service = service;

    /// <summary>Emite una póliza de auto.</summary>
    /// <remarks>
    /// Recibe el cliente, los datos del vehículo y las coberturas elegidas.
    ///
    /// **Reglas de negocio:**
    /// - La **PrimaTotal** se calcula en el servidor: suma de los montos de las coberturas seleccionadas.
    /// - La **SumaAsegurada** es el valor comercial del vehículo.
    /// - El **NumeroPoliza** se autogenera (formato POL-AAAA-000001).
    /// - Si la placa ya existe, se reutiliza el vehículo y se actualizan sus datos.
    /// - Los Ids de cobertura repetidos se consideran una sola vez.
    ///
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/polizas/emitir
    ///     {
    ///        "clienteId": 1,
    ///        "vehiculo": {
    ///           "placa": "M123456",
    ///           "marca": "Toyota",
    ///           "modelo": "Corolla",
    ///           "anio": 2022,
    ///           "valorComercial": 18500.00
    ///        },
    ///        "coberturaIds": [1, 2, 3]
    ///     }
    ///
    /// Con ese ejemplo la prima total es 6000.00 (1500 + 2500 + 2000).
    /// </remarks>
    /// <param name="request">Cliente, vehículo y coberturas seleccionadas.</param>
    /// <response code="201">Póliza emitida. El encabezado Location apunta a su detalle.</response>
    /// <response code="400">Datos inválidos, lista de coberturas vacía, cobertura inexistente o año de vehículo futuro.</response>
    /// <response code="404">El cliente indicado no existe.</response>
    [HttpPost("emitir")]
    [ProducesResponseType(typeof(PolizaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PolizaResponse>> Emitir(EmitirPolizaRequest request)
    {
        var poliza = await _service.EmitirAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = poliza.Id }, poliza);
    }

    /// <summary>Consulta el detalle de una póliza emitida.</summary>
    /// <remarks>Incluye cliente, vehículo y coberturas con los montos vigentes al emitir.</remarks>
    /// <param name="id">Identificador de la póliza.</param>
    /// <response code="200">Póliza encontrada.</response>
    /// <response code="404">No existe una póliza con ese Id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PolizaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PolizaResponse>> GetById(int id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Historial de todas las pólizas emitidas.</summary>
    /// <remarks>Ordenadas de la más reciente a la más antigua.</remarks>
    /// <response code="200">Lista de pólizas (puede estar vacía).</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<PolizaResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PolizaResponse>>> GetAll() =>
        Ok(await _service.GetAllAsync());
}