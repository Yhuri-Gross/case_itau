/* using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Data;
using Clientes.Api.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Clientes.Tests.Infrastructure.Repositories;

public class ClienteRepositoryTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAll_DeveRetornarTodosClientes()
    {
        using var context = CreateContext();

        context.Clientes.Add(new Cliente("Yhuri", "yhuri@email.com"));
        context.Clientes.Add(new Cliente("Maria", "maria@email.com"));
        await context.SaveChangesAsync();

        var repository = new ClienteRepository(context);

        var resultado = await repository.GetAll();

        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task GetById_DeveRetornarCliente()
    {
        using var context = CreateContext();

        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var repository = new ClienteRepository(context);

        var resultado = await repository.GetById(cliente.Id);

        Assert.NotNull(resultado);
        Assert.Equal("Yhuri", resultado.Nome);
    }

    [Fact]
    public async Task Add_DeveAdicionarCliente()
    {
        using var context = CreateContext();

        var repository = new ClienteRepository(context);

        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        await repository.Add(cliente);
        await repository.Save();

        Assert.Single(context.Clientes);
    }

    [Fact]
    public async Task Update_DeveAtualizarCliente()
    {
        using var context = CreateContext();

        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var repository = new ClienteRepository(context);

        cliente.Atualizar("Novo Nome", "novo@email.com");

        await repository.Update(cliente);
        await repository.Save();

        var clienteAtualizado = await context.Clientes.FindAsync(cliente.Id);

        Assert.Equal("Novo Nome", clienteAtualizado.Nome);
        Assert.Equal("novo@email.com", clienteAtualizado.Email);
    }

    [Fact]
    public async Task Delete_DeveRemoverCliente()
    {
        using var context = CreateContext();

        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        var repository = new ClienteRepository(context);

        await repository.Delete(cliente);
        await repository.Save();

        Assert.Empty(context.Clientes);
    }

    [Fact]
    public async Task Save_DevePersistirAlteracoes()
    {
        using var context = CreateContext();

        var repository = new ClienteRepository(context);

        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        await repository.Add(cliente);
        await repository.Save();

        var resultado = await context.Clientes.FirstOrDefaultAsync();

        Assert.NotNull(resultado);
        Assert.Equal("Yhuri", resultado.Nome);
    }
} */