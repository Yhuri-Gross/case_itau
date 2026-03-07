using Clientes.Api.Application.DTOs;
using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Repositories;

namespace Clientes.Api.Application.Services;

public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;

    public ClienteService(IClienteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ClienteResponseDto>> GetAll()
    {
        var clientes = await _repository.GetAll();

        return clientes.Select(c => new ClienteResponseDto
        {
            Id = c.Id,
            Nome = c.Nome,
            Email = c.Email,
            Saldo = c.Saldo
        }).ToList();
    }

    public async Task<ClienteResponseDto?> GetById(int id)
    {
        var cliente = await _repository.GetById(id);

        if (cliente == null)
            return null;

        return new ClienteResponseDto
        {
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Saldo = cliente.Saldo
        };
    }

    public async Task<int> Create(ClienteCreateDto dto)
    {
        var cliente = new Cliente(dto.Nome, dto.Email);

        await _repository.Add(cliente);
        await _repository.Save();

        return cliente.Id;
    }

    public async Task Update(int id, ClienteUpdateDto dto)
    {
        var cliente = await _repository.GetById(id);

        if (cliente == null)
            throw new Exception("Cliente não encontrado");

        cliente.Atualizar(dto.Nome, dto.Email);

        await _repository.Update(cliente);
        await _repository.Save();
    }

    public async Task Delete(int id)
    {
        var cliente = await _repository.GetById(id);

        if (cliente == null)
            throw new Exception("Cliente não encontrado");

        await _repository.Delete(cliente);
        await _repository.Save();
    }

    public async Task Depositar(int id, decimal valor)
    {
        var cliente = await _repository.GetById(id);

        if (cliente == null)
            throw new Exception("Cliente não encontrado");

        cliente.Depositar(valor);

        await _repository.Save();
    }

    public async Task Sacar(int id, decimal valor)
    {
        var cliente = await _repository.GetById(id);

        if (cliente == null)
            throw new Exception("Cliente não encontrado");

        cliente.Sacar(valor);

        await _repository.Save();
    }
}