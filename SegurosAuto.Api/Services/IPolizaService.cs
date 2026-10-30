using SegurosAuto.Api.Dtos;

namespace SegurosAuto.Api.Services;

/// <summary>Lógica de negocio de pólizas.</summary>
public interface IPolizaService
{
    /// <summary>Emite una póliza validando cliente, coberturas y vehículo, y calculando la prima.</summary>
    Task<PolizaResponse> EmitirAsync(EmitirPolizaRequest request);

    /// <summary>Devuelve el detalle de una póliza.</summary>
    Task<PolizaResponse> GetByIdAsync(int id);

    /// <summary>Devuelve el historial de pólizas emitidas.</summary>
    Task<List<PolizaResponse>> GetAllAsync();
}