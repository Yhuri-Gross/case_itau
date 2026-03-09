using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace Clientes.Tests.Infrastructure.Repositories;

public class IClienteRepositoryTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;

    public IClienteRepositoryTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaDeClientes()
    {
        var clientes = new List<Cliente>
        {
            new Cliente("Yhuri", "yhuri@email.com"),
            new Cliente("Maria", "maria@email.com")
        };

        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(clientes);

        var resultado = await _repositoryMock.Object.GetAll();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task GetById_DeveRetornarCliente()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(cliente);

        var resultado = await _repositoryMock.Object.GetById(1);

        Assert.NotNull(resultado);
        Assert.Equal("Yhuri", resultado.Nome);
    }

    [Fact]
    public async Task Add_DeveExecutarSemErro()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        _repositoryMock.Setup(r => r.Add(cliente)).Returns(Task.CompletedTask);

        await _repositoryMock.Object.Add(cliente);

        _repositoryMock.Verify(r => r.Add(cliente), Times.Once);
    }

    [Fact]
    public async Task Update_DeveExecutarSemErro()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        _repositoryMock.Setup(r => r.Update(cliente)).Returns(Task.CompletedTask);

        await _repositoryMock.Object.Update(cliente);

        _repositoryMock.Verify(r => r.Update(cliente), Times.Once);
    }

    [Fact]
    public async Task Delete_DeveExecutarSemErro()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        _repositoryMock.Setup(r => r.Delete(cliente)).Returns(Task.CompletedTask);

        await _repositoryMock.Object.Delete(cliente);

        _repositoryMock.Verify(r => r.Delete(cliente), Times.Once);
    }

    [Fact]
    public async Task Save_DeveExecutarSemErro()
    {
        _repositoryMock.Setup(r => r.Save()).Returns(Task.CompletedTask);

        await _repositoryMock.Object.Save();

        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }
}