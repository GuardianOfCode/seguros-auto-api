using Microsoft.AspNetCore.Mvc;
using SegurosAuto.Api.Dtos;
using SegurosAuto.Api.Services;

namespace SegurosAuto.Api.Controllers;

/// <summary>
/// Catálogo de coberturas (CRUD completo): Robo, Choque, Responsabilidad Civil, etc.
/// </summary>
[ApiController]
[Route("api/coberturas")]
[Produces("application/json")]
public class CoberturasController : ControllerBase
{
    private readonly ICoberturaService _service;
    public CoberturasController(ICoberturaService service) => _service = service;

    /// <summary>Lista todas las coberturas disponibles.</summary>
    /// <remarks>Se usa para que el ejecutivo elija las coberturas al emitir una póliza.</remarks>
    /// <response code="200">Lista de coberturas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<CoberturaResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CoberturaResponse>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Obtiene una cobertura por su Id.</summary>
    /// <param name="id">Identificador de la cobertura.</param>
    /// <response code="200">Cobertura encontrada.</response>
    /// <response code="404">No existe una cobertura con ese Id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CoberturaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CoberturaResponse>> GetById(int id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Crea una cobertura nueva en el catálogo.</summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/coberturas
    ///     {
    ///        "nombre": "Cristales",
    ///        "montoCobertura": 400.00
    ///     }
    /// </remarks>
    /// <param name="request">Datos de la cobertura.</param>
    /// <response code="201">Cobertura creada.</response>
    /// <response code="400">Datos inválidos (nombre vacío o monto menor o igual a cero).</response>
    [HttpPost]
    [ProducesResponseType(typeof(CoberturaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CoberturaResponse>> Crear(GuardarCoberturaRequest request)
    {
        var cobertura = await _service.CrearAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = cobertura.Id }, cobertura);
    }

    /// <summary>Actualiza una cobertura existente.</summary>
    /// <remarks>
    /// Las pólizas ya emitidas NO se ven afectadas: cada póliza conserva el monto que
    /// tenía la cobertura al momento de emitirse.
    /// </remarks>
    /// <param name="id">Identificador de la cobertura.</param>
    /// <param name="request">Nuevos datos de la cobertura.</param>
    /// <response code="200">Cobertura actualizada.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="404">No existe una cobertura con ese Id.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CoberturaResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CoberturaResponse>> Actualizar(int id, GuardarCoberturaRequest request) =>
        Ok(await _service.ActualizarAsync(id, request));

    /// <summary>Elimina una cobertura del catálogo.</summary>
    /// <remarks>No se puede eliminar una cobertura que ya esté incluida en pólizas emitidas.</remarks>
    /// <param name="id">Identificador de la cobertura.</param>
    /// <response code="204">Cobertura eliminada.</response>
    /// <response code="400">La cobertura está en pólizas emitidas.</response>
    /// <response code="404">No existe una cobertura con ese Id.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _service.EliminarAsync(id);
        return NoContent();
    }
}