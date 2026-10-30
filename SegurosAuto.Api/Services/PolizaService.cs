using SegurosAuto.Api.Dtos;
using SegurosAuto.Api.Exceptions;
using SegurosAuto.Api.Models;
using SegurosAuto.Api.Repositories;

namespace SegurosAuto.Api.Services;

/// <inheritdoc cref="IPolizaService"/>
public class PolizaService : IPolizaService
{
    private readonly IPolizaRepository _polizas;
    private readonly IClienteRepository _clientes;
    private readonly ICoberturaRepository _coberturas;

    public PolizaService(IPolizaRepository polizas, IClienteRepository clientes, ICoberturaRepository coberturas)
    {
        _polizas = polizas;
        _clientes = clientes;
        _coberturas = coberturas;
    }

    /// <exception cref="NotFoundException">El cliente no existe.</exception>
    /// <exception cref="BusinessException">Cobertura inexistente o año de vehículo inválido.</exception>
    public async Task<PolizaResponse> EmitirAsync(EmitirPolizaRequest request)
    {
        // 1. Validar cliente
        var cliente = await _clientes.GetByIdAsync(request.ClienteId)
            ?? throw new NotFoundException($"El cliente con Id {request.ClienteId} no existe.");

        // 2. Validar coberturas (sin repetidos y todas existentes)
        var idsSolicitados = request.CoberturaIds.Distinct().ToList();
        var coberturas = await _coberturas.GetByIdsAsync(idsSolicitados);
        var idsInexistentes = idsSolicitados.Except(coberturas.Select(c => c.Id)).ToList();
        if (idsInexistentes.Count > 0)
            throw new BusinessException($"Las siguientes coberturas no existen: {string.Join(", ", idsInexistentes)}.");

        // 3. Validar año del vehículo
        if (request.Vehiculo.Anio > DateTime.UtcNow.Year + 1)
            throw new BusinessException("El año del vehículo no puede ser futuro.");

        // 4. Reusar el vehículo si la placa ya existe, o crear uno nuevo
        var placa = request.Vehiculo.Placa.Trim().ToUpperInvariant();
        var vehiculo = await _polizas.GetVehiculoByPlacaAsync(placa)
                       ?? new Vehiculo { Placa = placa };

        vehiculo.Marca = request.Vehiculo.Marca.Trim();
        vehiculo.Modelo = request.Vehiculo.Modelo.Trim();
        vehiculo.Anio = request.Vehiculo.Anio;
        vehiculo.ValorComercial = request.Vehiculo.ValorComercial;

        // 5. LÓGICA DE NEGOCIO: la prima se calcula en el servidor
        var primaTotal = coberturas.Sum(c => c.MontoCobertura);
        var sumaAsegurada = vehiculo.ValorComercial;

        var poliza = new Poliza
        {
            ClienteId = cliente.Id,
            Vehiculo = vehiculo,
            FechaEmision = DateTime.UtcNow,
            SumaAsegurada = sumaAsegurada,
            PrimaTotal = primaTotal,
            Coberturas = coberturas.Select(c => new PolizaCobertura
            {
                CoberturaId = c.Id,
                MontoCobertura = c.MontoCobertura // se congela el monto vigente
            }).ToList()
        };

        await _polizas.AddAsync(poliza);

        // Recargar con todo el detalle para responder
        var guardada = await _polizas.GetByIdAsync(poliza.Id)
            ?? throw new NotFoundException("No se pudo recuperar la póliza emitida.");

        return ToResponse(guardada);
    }

    /// <exception cref="NotFoundException">La póliza no existe.</exception>
    public async Task<PolizaResponse> GetByIdAsync(int id)
    {
        var poliza = await _polizas.GetByIdAsync(id)
            ?? throw new NotFoundException($"La póliza con Id {id} no existe.");
        return ToResponse(poliza);
    }

    public async Task<List<PolizaResponse>> GetAllAsync()
    {
        var polizas = await _polizas.GetAllAsync();
        return polizas.Select(ToResponse).ToList();
    }

    // Convierte la entidad en el DTO que se devuelve al cliente de la API
    private static PolizaResponse ToResponse(Poliza p) => new()
    {
        Id = p.Id,
        NumeroPoliza = p.NumeroPoliza,
        FechaEmision = p.FechaEmision,
        SumaAsegurada = p.SumaAsegurada,
        PrimaTotal = p.PrimaTotal,
        Cliente = new ClienteResponse
        {
            Id = p.Cliente.Id,
            Nombre = p.Cliente.Nombre,
            Identificacion = p.Cliente.Identificacion,
            Correo = p.Cliente.Correo
        },
        Vehiculo = new VehiculoResponse
        {
            Id = p.Vehiculo.Id,
            Placa = p.Vehiculo.Placa,
            Marca = p.Vehiculo.Marca,
            Modelo = p.Vehiculo.Modelo,
            Anio = p.Vehiculo.Anio,
            ValorComercial = p.Vehiculo.ValorComercial
        },
        Coberturas = p.Coberturas.Select(pc => new CoberturaResponse
        {
            Id = pc.CoberturaId,
            Nombre = pc.Cobertura.Nombre,
            MontoCobertura = pc.MontoCobertura
        }).ToList()
    };
}