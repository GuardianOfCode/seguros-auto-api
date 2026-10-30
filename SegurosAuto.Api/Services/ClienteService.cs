using SegurosAuto.Api.Dtos;
using SegurosAuto.Api.Exceptions;
using SegurosAuto.Api.Models;
using SegurosAuto.Api.Repositories;

namespace SegurosAuto.Api.Services;

/// <summary>Lógica de negocio de clientes (CRUD).</summary>
public interface IClienteService
{
    /// <summary>Lista todos los clientes.</summary>
    Task<List<ClienteResponse>> GetAllAsync();

    /// <summary>Obtiene un cliente por Id.</summary>
    Task<ClienteResponse> GetByIdAsync(int id);

    /// <summary>Crea un cliente validando que la identificación no esté repetida.</summary>
    Task<ClienteResponse> CrearAsync(CrearClienteRequest request);

    /// <summary>Actualiza un cliente validando que la identificación no pertenezca a otro.</summary>
    Task<ClienteResponse> ActualizarAsync(int id, ActualizarClienteRequest request);

    /// <summary>Elimina un cliente que no tenga pólizas emitidas.</summary>
    Task EliminarAsync(int id);
}

/// <inheritdoc cref="IClienteService"/>
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _clientes;
    public ClienteService(IClienteRepository clientes) => _clientes = clientes;

    public async Task<List<ClienteResponse>> GetAllAsync() =>
        (await _clientes.GetAllAsync()).Select(ToResponse).ToList();

    /// <exception cref="NotFoundException">El cliente no existe.</exception>
    public async Task<ClienteResponse> GetByIdAsync(int id)
    {
        var cliente = await _clientes.GetByIdAsync(id)
            ?? throw new NotFoundException($"El cliente con Id {id} no existe.");
        return ToResponse(cliente);
    }

    /// <exception cref="BusinessException">La identificación ya existe.</exception>
    public async Task<ClienteResponse> CrearAsync(CrearClienteRequest request)
    {
        var identificacion = request.Identificacion.Trim();

        if (await _clientes.ExistsByIdentificacionAsync(identificacion))
            throw new BusinessException($"Ya existe un cliente con la identificación {identificacion}.");

        var cliente = new Cliente
        {
            Nombre = request.Nombre.Trim(),
            Identificacion = identificacion,
            Correo = request.Correo.Trim()
        };

        await _clientes.AddAsync(cliente);
        return ToResponse(cliente);
    }

    /// <exception cref="NotFoundException">El cliente no existe.</exception>
    /// <exception cref="BusinessException">La identificación pertenece a otro cliente.</exception>
    public async Task<ClienteResponse> ActualizarAsync(int id, ActualizarClienteRequest request)
    {
        var cliente = await _clientes.GetByIdAsync(id)
            ?? throw new NotFoundException($"El cliente con Id {id} no existe.");

        var identificacion = request.Identificacion.Trim();

        if (await _clientes.ExistsByIdentificacionAsync(identificacion, excludeId: id))
            throw new BusinessException($"Ya existe otro cliente con la identificación {identificacion}.");

        cliente.Nombre = request.Nombre.Trim();
        cliente.Identificacion = identificacion;
        cliente.Correo = request.Correo.Trim();

        await _clientes.UpdateAsync(cliente);
        return ToResponse(cliente);
    }

    /// <exception cref="NotFoundException">El cliente no existe.</exception>
    /// <exception cref="BusinessException">El cliente tiene pólizas emitidas.</exception>
    public async Task EliminarAsync(int id)
    {
        var cliente = await _clientes.GetByIdAsync(id)
            ?? throw new NotFoundException($"El cliente con Id {id} no existe.");

        if (await _clientes.HasPolizasAsync(id))
            throw new BusinessException("No se puede eliminar el cliente porque tiene pólizas emitidas.");

        await _clientes.DeleteAsync(cliente);
    }

    private static ClienteResponse ToResponse(Cliente c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Identificacion = c.Identificacion,
        Correo = c.Correo
    };
}