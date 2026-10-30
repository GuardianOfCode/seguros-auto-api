using SegurosAuto.Api.Models;

namespace SegurosAuto.Api.Repositories;

/// <summary>Acceso a datos del catálogo de coberturas.</summary>
public interface ICoberturaRepository
{
    /// <summary>Lista todas las coberturas ordenadas por nombre.</summary>
    Task<List<Cobertura>> GetAllAsync();

    /// <summary>Busca una cobertura por Id; devuelve null si no existe.</summary>
    Task<Cobertura?> GetByIdAsync(int id);

    /// <summary>Devuelve las coberturas cuyos Ids estén en la lista indicada.</summary>
    Task<List<Cobertura>> GetByIdsAsync(IEnumerable<int> ids);

    /// <summary>Indica si alguna póliza emitida usa esta cobertura.</summary>
    Task<bool> IsInUseAsync(int coberturaId);

    /// <summary>Inserta una cobertura.</summary>
    Task AddAsync(Cobertura cobertura);

    /// <summary>Actualiza una cobertura.</summary>
    Task UpdateAsync(Cobertura cobertura);

    /// <summary>Elimina una cobertura.</summary>
    Task DeleteAsync(Cobertura cobertura);
}