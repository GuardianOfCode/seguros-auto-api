using Microsoft.AspNetCore.Mvc;
using SegurosAuto.Api.Dtos;
using SegurosAuto.Api.Services;

namespace SegurosAuto.Api.Controllers;

/// <summary>
/// Administración de clientes (CRUD completo): consultar, crear, actualizar y eliminar.
/// </summary>
[ApiController]
[Route("api/clientes")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;
    public ClientesController(IClienteService service) => _service = service;

    /// <summary>Lista todos los clientes.</summary>
    /// <remarks>Catálogo de clientes ordenado alfabéticamente por nombre.</remarks>
    /// <response code="200">Lista de clientes (puede estar vacía).</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClienteResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClienteResponse>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    /// <summary>Obtiene un cliente por su Id.</summary>
    /// <param name="id">Identificador del cliente.</param>
    /// <response code="200">Cliente encontrado.</response>
    /// <response code="404">No existe un cliente con ese Id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteResponse>> GetById(int id) =>
        Ok(await _service.GetByIdAsync(id));

    /// <summary>Registra un cliente nuevo.</summary>
    /// <remarks>
    /// Ejemplo de solicitud:
    ///
    ///     POST /api/clientes
    ///     {
    ///        "nombre": "Carlos Mendoza",
    ///        "identificacion": "001-200380-0003C",
    ///        "correo": "carlos.mendoza@correo.com"
    ///     }
    ///
    /// La identificación debe ser única.
    /// </remarks>
    /// <param name="request">Datos del cliente.</param>
    /// <response code="201">Cliente creado. El encabezado Location apunta al nuevo recurso.</response>
    /// <response code="400">Datos inválidos o identificación ya registrada.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClienteResponse>> Crear(CrearClienteRequest request)
    {
        var cliente = await _service.CrearAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    /// <summary>Actualiza los datos de un cliente existente.</summary>
    /// <param name="id">Identificador del cliente a actualizar.</param>
    /// <param name="request">Nuevos datos del cliente.</param>
    /// <response code="200">Cliente actualizado.</response>
    /// <response code="400">Datos inválidos o la identificación pertenece a otro cliente.</response>
    /// <response code="404">No existe un cliente con ese Id.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteResponse>> Actualizar(int id, ActualizarClienteRequest request) =>
        Ok(await _service.ActualizarAsync(id, request));

    /// <summary>Elimina un cliente.</summary>
    /// <remarks>No se puede eliminar un cliente que ya tenga pólizas emitidas.</remarks>
    /// <param name="id">Identificador del cliente a eliminar.</param>
    /// <response code="204">Cliente eliminado.</response>
    /// <response code="400">El cliente tiene pólizas emitidas.</response>
    /// <response code="404">No existe un cliente con ese Id.</response>
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