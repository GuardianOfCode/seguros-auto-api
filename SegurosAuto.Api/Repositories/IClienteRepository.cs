using SegurosAuto.Api.Models;

namespace SegurosAuto.Api.Repositories;

/// <summary>Acceso a datos de la tabla Clientes.</summary>
public interface IClienteRepository
{
    /// <summary>Lista todos los clientes ordenados por nombre.</summary>
    Task<List<Cliente>> GetAllAsync();

    /// <summary>Busca un cliente por Id; devuelve null si no existe.</summary>
    Task<Cliente?> GetByIdAsync(int id);

    /// <summary>
    /// Indica si ya existe una identificación. Con <paramref name="excludeId"/> se ignora
    /// al propio cliente (útil al actualizar).
    /// </summary>
    Task<bool> ExistsByIdentificacionAsync(string identificacion, int? excludeId = null);

    /// <summary>Indica si el cliente tiene pólizas emitidas.</summary>
    Task<bool> HasPolizasAsync(int clienteId);

    /// <summary>Inserta un cliente.</summary>
    Task AddAsync(Cliente cliente);

    /// <summary>Actualiza un cliente.</summary>
    Task UpdateAsync(Cliente cliente);

    /// <summary>Elimina un cliente.</summary>
    Task DeleteAsync(Cliente cliente);
}