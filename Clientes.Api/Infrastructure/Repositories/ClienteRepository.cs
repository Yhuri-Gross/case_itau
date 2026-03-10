using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Clientes.Api.Infrastructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Cliente>> GetAll()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> GetById(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<Cliente?> GetByEmail(string email)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task Add(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }

    public Task Update(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

    public Task Delete(Cliente cliente)
    {
        _context.Clientes.Remove(cliente);
        return Task.CompletedTask;
    }

    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
}