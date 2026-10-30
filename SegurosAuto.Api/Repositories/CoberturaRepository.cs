using Microsoft.EntityFrameworkCore;
using SegurosAuto.Api.Data;
using SegurosAuto.Api.Models;

namespace SegurosAuto.Api.Repositories;

/// <inheritdoc cref="ICoberturaRepository"/>
public class CoberturaRepository : ICoberturaRepository
{
    private readonly AppDbContext _db;
    public CoberturaRepository(AppDbContext db) => _db = db;

    public Task<List<Cobertura>> GetAllAsync() =>
        _db.Coberturas.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync();

    public Task<Cobertura?> GetByIdAsync(int id) =>
        _db.Coberturas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public Task<List<Cobertura>> GetByIdsAsync(IEnumerable<int> ids) =>
        _db.Coberturas.AsNoTracking().Where(c => ids.Contains(c.Id)).ToListAsync();

    public Task<bool> IsInUseAsync(int coberturaId) =>
        _db.PolizaCoberturas.AnyAsync(pc => pc.CoberturaId == coberturaId);

    public async Task AddAsync(Cobertura cobertura)
    {
        _db.Coberturas.Add(cobertura);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cobertura cobertura)
    {
        _db.Coberturas.Update(cobertura);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Cobertura cobertura)
    {
        _db.Coberturas.Remove(cobertura);
        await _db.SaveChangesAsync();
    }
}