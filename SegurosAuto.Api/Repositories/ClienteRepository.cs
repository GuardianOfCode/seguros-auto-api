using Microsoft.EntityFrameworkCore;
using SegurosAuto.Api.Data;
using SegurosAuto.Api.Models;

namespace SegurosAuto.Api.Repositories;

/// <inheritdoc cref="IClienteRepository"/>
public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _db;
    public ClienteRepository(AppDbContext db) => _db = db;

    public Task<List<Cliente>> GetAllAsync() =>
        _db.Clientes.AsNoTracking().OrderBy(c => c.Nombre).ToListAsync();

    public Task<Cliente?> GetByIdAsync(int id) =>
        _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public Task<bool> ExistsByIdentificacionAsync(string identificacion, int? excludeId = null) =>
        _db.Clientes.AnyAsync(c => c.Identificacion == identificacion
                                   && (excludeId == null || c.Id != excludeId));

    public Task<bool> HasPolizasAsync(int clienteId) =>
        _db.Polizas.AnyAsync(p => p.ClienteId == clienteId);

    public async Task AddAsync(Cliente cliente)
    {
        _db.Clientes.Add(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cliente cliente)
    {
        _db.Clientes.Update(cliente);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Cliente cliente)
    {
        _db.Clientes.Remove(cliente);
        await _db.SaveChangesAsync();
    }
}