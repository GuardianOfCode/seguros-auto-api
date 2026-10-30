using SegurosAuto.Api.Models;

namespace SegurosAuto.Api.Repositories;

/// <summary>Acceso a datos de pólizas y vehículos.</summary>
public interface IPolizaRepository
{
    /// <summary>Busca una póliza (con cliente, vehículo y coberturas) por Id; null si no existe.</summary>
    Task<Poliza?> GetByIdAsync(int id);

    /// <summary>Lista todas las pólizas emitidas, de la más reciente a la más antigua.</summary>
    Task<List<Poliza>> GetAllAsync();

    /// <summary>
    /// Guarda la póliza (y su vehículo si es nuevo) en una sola transacción
    /// y le asigna el NumeroPoliza autogenerado.
    /// </summary>
    Task<Poliza> AddAsync(Poliza poliza);

    /// <summary>Busca un vehículo por placa; null si no existe.</summary>
    Task<Vehiculo?> GetVehiculoByPlacaAsync(string placa);
}