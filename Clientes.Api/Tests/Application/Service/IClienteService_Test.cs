using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Moq;
using Xunit;

namespace Clientes.Tests.Application.Services;

public class IClienteServiceTests
{
    private readonly Mock<IClienteService> _serviceMock;

    public IClienteServiceTests()
    {
        _serviceMock = new Mock<IClienteService>();
    }

    [Fact]
    public async Task GetAll_DeveRetornarListaDeClientes()
    {
        var clientes = new List<ClienteResponseDto>
        {
            new ClienteResponseDto { Id = 1, Nome = "Yhuri", Email = "yhuri@email.com", Saldo = 100 },
            new ClienteResponseDto { Id = 2, Nome = "Maria", Email = "maria@email.com", Saldo = 200 }
        };

        _serviceMock.Setup(s => s.GetAll()).ReturnsAsync(clientes);

        var resultado = await _serviceMock.Object.GetAll();

        Assert.NotNull(resultado);
        Assert.Equal(2, resultado.Count);
    }

    [Fact]
    public async Task GetById_DeveRetornarCliente()
    {
        var cliente = new ClienteResponseDto
        {
            Id = 1,
            Nome = "Yhuri",
            Email = "yhuri@email.com",
            Saldo = 100
        };

        _serviceMock.Setup(s => s.GetById(1)).ReturnsAsync(cliente);

        var resultado = await _serviceMock.Object.GetById(1);

        Assert.NotNull(resultado);
        Assert.Equal("Yhuri", resultado.Nome);
    }

    [Fact]
    public async Task Create_DeveRetornarId()
    {
        var dto = new ClienteCreateDto
        {
            Nome = "Yhuri",
            Email = "yhuri@email.com"
        };

        _serviceMock.Setup(s => s.Create(dto)).ReturnsAsync(1);

        var resultado = await _serviceMock.Object.Create(dto);

        Assert.Equal(1, resultado);
    }

    [Fact]
    public async Task Update_DeveExecutarSemErro()
    {
        var dto = new ClienteUpdateDto
        {
            Nome = "Novo Nome",
            Email = "novo@email.com"
        };

        _serviceMock.Setup(s => s.Update(1, dto)).Returns(Task.CompletedTask);

        await _serviceMock.Object.Update(1, dto);

        _serviceMock.Verify(s => s.Update(1, dto), Times.Once);
    }

    [Fact]
    public async Task Delete_DeveExecutarSemErro()
    {
        _serviceMock.Setup(s => s.Delete(1)).Returns(Task.CompletedTask);

        await _serviceMock.Object.Delete(1);

        _serviceMock.Verify(s => s.Delete(1), Times.Once);
    }

    [Fact]
    public async Task Depositar_DeveExecutarSemErro()
    {
        _serviceMock.Setup(s => s.Depositar(1, 100)).Returns(Task.CompletedTask);

        await _serviceMock.Object.Depositar(1, 100);

        _serviceMock.Verify(s => s.Depositar(1, 100), Times.Once);
    }

    [Fact]
    public async Task Sacar_DeveExecutarSemErro()
    {
        _serviceMock.Setup(s => s.Sacar(1, 50)).Returns(Task.CompletedTask);

        await _serviceMock.Object.Sacar(1, 50);

        _serviceMock.Verify(s => s.Sacar(1, 50), Times.Once);
    }
}