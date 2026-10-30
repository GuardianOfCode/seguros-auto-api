using SegurosAuto.Api.Dtos;
using SegurosAuto.Api.Exceptions;
using SegurosAuto.Api.Models;
using SegurosAuto.Api.Repositories;

namespace SegurosAuto.Api.Services;

/// <summary>Lógica de negocio del catálogo de coberturas (CRUD).</summary>
public interface ICoberturaService
{
    /// <summary>Lista todas las coberturas.</summary>
    Task<List<CoberturaResponse>> GetAllAsync();

    /// <summary>Obtiene una cobertura por Id.</summary>
    Task<CoberturaResponse> GetByIdAsync(int id);

    /// <summary>Crea una cobertura.</summary>
    Task<CoberturaResponse> CrearAsync(GuardarCoberturaRequest request);

    /// <summary>Actualiza una cobertura (no altera pólizas ya emitidas).</summary>
    Task<CoberturaResponse> ActualizarAsync(int id, GuardarCoberturaRequest request);

    /// <summary>Elimina una cobertura que no esté en ninguna póliza.</summary>
    Task EliminarAsync(int id);
}

/// <inheritdoc cref="ICoberturaService"/>
public class CoberturaService : ICoberturaService
{
    private readonly ICoberturaRepository _coberturas;
    public CoberturaService(ICoberturaRepository coberturas) => _coberturas = coberturas;

    public async Task<List<CoberturaResponse>> GetAllAsync() =>
        (await _coberturas.GetAllAsync()).Select(ToResponse).ToList();

    /// <exception cref="NotFoundException">La cobertura no existe.</exception>
    public async Task<CoberturaResponse> GetByIdAsync(int id)
    {
        var cobertura = await _coberturas.GetByIdAsync(id)
            ?? throw new NotFoundException($"La cobertura con Id {id} no existe.");
        return ToResponse(cobertura);
    }

    public async Task<CoberturaResponse> CrearAsync(GuardarCoberturaRequest request)
    {
        var cobertura = new Cobertura
        {
            Nombre = request.Nombre.Trim(),
            MontoCobertura = request.MontoCobertura
        };

        await _coberturas.AddAsync(cobertura);
        return ToResponse(cobertura);
    }

    /// <exception cref="NotFoundException">La cobertura no existe.</exception>
    public async Task<CoberturaResponse> ActualizarAsync(int id, GuardarCoberturaRequest request)
    {
        var cobertura = await _coberturas.GetByIdAsync(id)
            ?? throw new NotFoundException($"La cobertura con Id {id} no existe.");

        // Las pólizas ya emitidas NO cambian: guardan su propio monto en PolizaCobertura
        cobertura.Nombre = request.Nombre.Trim();
        cobertura.MontoCobertura = request.MontoCobertura;

        await _coberturas.UpdateAsync(cobertura);
        return ToResponse(cobertura);
    }

    /// <exception cref="NotFoundException">La cobertura no existe.</exception>
    /// <exception cref="BusinessException">La cobertura está en pólizas emitidas.</exception>
    public async Task EliminarAsync(int id)
    {
        var cobertura = await _coberturas.GetByIdAsync(id)
            ?? throw new NotFoundException($"La cobertura con Id {id} no existe.");

        if (await _coberturas.IsInUseAsync(id))
            throw new BusinessException("No se puede eliminar la cobertura porque está en pólizas emitidas.");

        await _coberturas.DeleteAsync(cobertura);
    }

    private static CoberturaResponse ToResponse(Cobertura c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        MontoCobertura = c.MontoCobertura
    };
}