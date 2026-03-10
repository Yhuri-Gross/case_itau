using Clientes.Api.Application.DTOs;

namespace Clientes.Api.Application.Services;

public interface IClienteService
{
    Task<List<ClienteResponseDto>> GetAll();

    Task<ClienteResponseDto?> GetById(int id);

    Task<int> Create(ClienteCreateDto dto);

    Task Update(int id, ClienteUpdateDto dto);

    Task Delete(int id);

    Task Depositar(int id, decimal valor);

    Task Sacar(int id, decimal valor);
}