using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace Clientes.Tests.Application.Services;

public class ClienteServiceTests
{
    private readonly Mock<IClienteRepository> _repositoryMock;
    private readonly ClienteService _service;

    public ClienteServiceTests()
    {
        _repositoryMock = new Mock<IClienteRepository>();
        _service = new ClienteService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaDeClientes()
    {
        var clientes = new List<Cliente>
        {
            new Cliente("Yhuri", "yhuri@email.com"),
            new Cliente("Maria", "maria@email.com")
        };

        _repositoryMock.Setup(r => r.GetAll())
            .ReturnsAsync(clientes);

        var resultado = await _service.GetAll();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
        Assert.Equal("Yhuri", resultado[0].Nome);
        Assert.Equal("Maria", resultado[1].Nome);
    }

    [Fact]
    public async Task GetById_DeveRetornarCliente_QuandoExistir()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync(cliente);

        var resultado = await _service.GetById(1);

        Assert.NotNull(resultado);
        Assert.Equal("Yhuri", resultado.Nome);
        Assert.Equal("yhuri@email.com", resultado.Email);
    }

    [Fact]
    public async Task GetById_DeveRetornarNull_QuandoClienteNaoExistir()
    {
        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync((Cliente?)null);

        var resultado = await _service.GetById(1);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task Create_DeveAdicionarClienteESalvar()
    {
        var dto = new ClienteCreateDto
        {
            Nome = "Yhuri",
            Email = "yhuri@email.com"
        };

        _repositoryMock.Setup(r => r.Add(It.IsAny<Cliente>()))
            .Returns(Task.CompletedTask);

        _repositoryMock.Setup(r => r.Save())
            .Returns(Task.CompletedTask);

        var id = await _service.Create(dto);

        _repositoryMock.Verify(r => r.Add(It.IsAny<Cliente>()), Times.Once);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Update_DeveAtualizarCliente_QuandoExistir()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        var dto = new ClienteUpdateDto
        {
            Nome = "Novo Nome",
            Email = "novo@email.com"
        };

        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync(cliente);

        _repositoryMock.Setup(r => r.Update(cliente))
            .Returns(Task.CompletedTask);

        _repositoryMock.Setup(r => r.Save())
            .Returns(Task.CompletedTask);

        await _service.Update(1, dto);

        _repositoryMock.Verify(r => r.Update(cliente), Times.Once);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Update_DeveLancarExcecao_QuandoClienteNaoExistir()
    {
        var dto = new ClienteUpdateDto
        {
            Nome = "Novo Nome",
            Email = "novo@email.com"
        };

        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync((Cliente?)null);

        await Assert.ThrowsAsync<Exception>(() => _service.Update(1, dto));
    }

    [Fact]
    public async Task Delete_DeveRemoverCliente_QuandoExistir()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync(cliente);

        _repositoryMock.Setup(r => r.Delete(cliente))
            .Returns(Task.CompletedTask);

        _repositoryMock.Setup(r => r.Save())
            .Returns(Task.CompletedTask);

        await _service.Delete(1);

        _repositoryMock.Verify(r => r.Delete(cliente), Times.Once);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Delete_DeveLancarExcecao_QuandoClienteNaoExistir()
    {
        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync((Cliente?)null);

        await Assert.ThrowsAsync<Exception>(() => _service.Delete(1));
    }

    [Fact]
    public async Task Depositar_DeveSalvarAlteracao_QuandoClienteExistir()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync(cliente);

        _repositoryMock.Setup(r => r.Save())
            .Returns(Task.CompletedTask);

        await _service.Depositar(1, 100);

        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Depositar_DeveLancarExcecao_QuandoClienteNaoExistir()
    {
        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync((Cliente?)null);

        await Assert.ThrowsAsync<Exception>(() => _service.Depositar(1, 100));
    }

    [Fact]
    public async Task Sacar_DeveSalvarAlteracao_QuandoClienteExistir()
    {
        var cliente = new Cliente("Teste", "teste@email.com");

        cliente.Depositar(100);

        _repositoryMock.Setup(x => x.GetById(1))
            .ReturnsAsync(cliente);

        _repositoryMock.Setup(x => x.Save())
            .Returns(Task.CompletedTask);

        await _service.Sacar(1, 50);

        _repositoryMock.Verify(x => x.Save(), Times.Once);
    }

    [Fact]
    public async Task Sacar_DeveLancarExcecao_QuandoClienteNaoExistir()
    {
        _repositoryMock.Setup(r => r.GetById(1))
            .ReturnsAsync((Cliente?)null);

        await Assert.ThrowsAsync<Exception>(() => _service.Sacar(1, 50));
    }
}