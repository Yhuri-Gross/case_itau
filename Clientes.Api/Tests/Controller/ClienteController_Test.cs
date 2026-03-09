using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Clientes.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Clientes.Tests.Controllers;

public class ClientesControllerTests
{
    private readonly Mock<IClienteService> _serviceMock;
    private readonly ClientesController _controller;

    public ClientesControllerTests()
    {
        _serviceMock = new Mock<IClienteService>();
        _controller = new ClientesController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_DeveRetornarOkComListaDeClientes()
    {
        var clientes = new List<ClienteResponseDto>
        {
            new ClienteResponseDto { Id = 1, Nome = "Yhuri", Email = "yhuri@email.com", Saldo = 100 },
            new ClienteResponseDto { Id = 2, Nome = "Maria", Email = "maria@email.com", Saldo = 200 }
        };

        _serviceMock.Setup(s => s.GetAll()).ReturnsAsync(clientes);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsType<List<ClienteResponseDto>>(okResult.Value);
        Assert.Equal(2, retorno.Count);
    }

    [Fact]
    public async Task GetById_DeveRetornarOk_QuandoClienteExistir()
    {
        var cliente = new ClienteResponseDto
        {
            Id = 1,
            Nome = "Yhuri",
            Email = "yhuri@email.com",
            Saldo = 100
        };

        _serviceMock.Setup(s => s.GetById(1)).ReturnsAsync(cliente);

        var result = await _controller.GetById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsType<ClienteResponseDto>(okResult.Value);
        Assert.Equal("Yhuri", retorno.Nome);
    }

    [Fact]
    public async Task GetById_DeveRetornarNotFound_QuandoClienteNaoExistir()
    {
        _serviceMock.Setup(s => s.GetById(1)).ReturnsAsync((ClienteResponseDto?)null);

        var result = await _controller.GetById(1);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_DeveRetornarCreatedAtAction()
    {
        var dto = new ClienteCreateDto
        {
            Nome = "Yhuri",
            Email = "yhuri@email.com"
        };

        _serviceMock.Setup(s => s.Create(dto)).ReturnsAsync(1);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(ClientesController.GetById), createdResult.ActionName);
    }

    [Fact]
    public async Task Update_DeveRetornarNoContent()
    {
        var dto = new ClienteUpdateDto
        {
            Nome = "Novo Nome",
            Email = "novo@email.com"
        };

        _serviceMock.Setup(s => s.Update(1, dto)).Returns(Task.CompletedTask);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_DeveRetornarNoContent()
    {
        _serviceMock.Setup(s => s.Delete(1)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Depositar_DeveRetornarOk()
    {
        var dto = new OperacaoSaldoDto
        {
            Valor = 100
        };

        _serviceMock.Setup(s => s.Depositar(1, dto.Valor)).Returns(Task.CompletedTask);

        var result = await _controller.Depositar(1, dto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Sacar_DeveRetornarOk()
    {
        var dto = new OperacaoSaldoDto
        {
            Valor = 50
        };

        _serviceMock.Setup(s => s.Sacar(1, dto.Valor)).Returns(Task.CompletedTask);

        var result = await _controller.Sacar(1, dto);

        Assert.IsType<OkResult>(result);
    }
}