using Microsoft.EntityFrameworkCore;
using SegurosAuto.Api.Data;
using SegurosAuto.Api.Models;

namespace SegurosAuto.Api.Repositories;

/// <inheritdoc cref="IPolizaRepository"/>
public class PolizaRepository : IPolizaRepository
{
    private readonly AppDbContext _db;
    public PolizaRepository(AppDbContext db) => _db = db;

    // Consulta base que trae la póliza con todas sus relaciones
    private IQueryable<Poliza> ConDetalle() =>
        _db.Polizas
            .Include(p => p.Cliente)
            .Include(p => p.Vehiculo)
            .Include(p => p.Coberturas).ThenInclude(pc => pc.Cobertura);

    public Task<Poliza?> GetByIdAsync(int id) =>
        ConDetalle().AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public Task<List<Poliza>> GetAllAsync() =>
        ConDetalle().AsNoTracking().OrderByDescending(p => p.FechaEmision).ToListAsync();

    public Task<Vehiculo?> GetVehiculoByPlacaAsync(string placa) =>
        _db.Vehiculos.FirstOrDefaultAsync(v => v.Placa == placa);

    public async Task<Poliza> AddAsync(Poliza poliza)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();

        // 1) Número temporal único (la columna es NOT NULL + UNIQUE)
        poliza.NumeroPoliza = "TMP-" + Guid.NewGuid().ToString("N")[..20];
        _db.Polizas.Add(poliza);
        await _db.SaveChangesAsync(); // aquí SQL Server genera el Id

        // 2) Número definitivo basado en el Id: POL-2026-000001
        poliza.NumeroPoliza = $"POL-{poliza.FechaEmision:yyyy}-{poliza.Id:D6}";
        await _db.SaveChangesAsync();

        await tx.CommitAsync();
        return poliza;
    }
}