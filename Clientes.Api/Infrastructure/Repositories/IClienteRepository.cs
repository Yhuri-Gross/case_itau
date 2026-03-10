using Clientes.Api.Domain.Entities;

namespace Clientes.Api.Infrastructure.Repositories;

public interface IClienteRepository
{
    Task<List<Cliente>> GetAll();
    Task<Cliente?> GetById(int id);
    Task<Cliente?> GetByEmail(string email);
    Task Add(Cliente cliente);
    Task Update(Cliente cliente);
    Task Delete(Cliente cliente);
    Task Save();
}