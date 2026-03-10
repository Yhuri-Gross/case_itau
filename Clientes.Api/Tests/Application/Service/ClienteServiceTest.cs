using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clientes.Api.Tests.Application;

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
    public async Task GetAll_Deve_Retornar_Lista_De_Clientes()
    {
        var clientes = new List<Cliente>
        {
            new("Maria", "maria@email.com", "123"),
            new("Joao", "joao@email.com", "456")
        };

        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(clientes);

        var resultado = await _service.GetAll();

        resultado.Should().HaveCount(2);
        resultado[0].Nome.Should().Be("Maria");
        resultado[1].Nome.Should().Be("Joao");
    }

    [Fact]
    public async Task GetById_Deve_Retornar_Cliente_Quando_Existir()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123");

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(cliente);

        var resultado = await _service.GetById(1);

        resultado.Should().NotBeNull();
        resultado!.Nome.Should().Be("Maria");
        resultado.Email.Should().Be("maria@email.com");
    }

    [Fact]
    public async Task GetById_Deve_Retornar_Null_Quando_Nao_Existir()
    {
        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync((Cliente?)null);

        var resultado = await _service.GetById(1);

        resultado.Should().BeNull();
    }

    [Fact]
    public async Task Create_Deve_Criar_Cliente_E_Salvar()
    {
        var dto = new ClienteCreateDto
        {
            Nome = "Maria",
            Email = "maria@email.com",
            Senha = "123456"
        };

        _repositoryMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync((Cliente?)null);

        var id = await _service.Create(dto);

        _repositoryMock.Verify(r => r.Add(It.IsAny<Cliente>()), Times.Once);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Create_Deve_Lancar_Excecao_Quando_Email_Ja_Existir()
    {
        var dto = new ClienteCreateDto
        {
            Nome = "Maria",
            Email = "maria@email.com",
            Senha = "123456"
        };

        _repositoryMock
            .Setup(r => r.GetByEmail(dto.Email))
            .ReturnsAsync(new Cliente("Outra", dto.Email, "123"));

        var act = async () => await _service.Create(dto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Já existe um cliente com este e-mail");
    }

    [Fact]
    public async Task Update_Deve_Atualizar_Cliente_Quando_Existir()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123");
        var dto = new ClienteUpdateDto
        {
            Nome = "Maria Silva",
            Email = "maria.silva@email.com"
        };

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(cliente);

        await _service.Update(1, dto);

        cliente.Nome.Should().Be("Maria Silva");
        cliente.Email.Should().Be("maria.silva@email.com");
        _repositoryMock.Verify(r => r.Update(cliente), Times.Once);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Update_Deve_Lancar_Excecao_Quando_Cliente_Nao_Existir()
    {
        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync((Cliente?)null);

        var dto = new ClienteUpdateDto
        {
            Nome = "Novo Nome",
            Email = "novo@email.com"
        };

        var act = async () => await _service.Update(1, dto);

        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Cliente não encontrado");
    }

    [Fact]
    public async Task Depositar_Deve_Alterar_Saldo_E_Salvar()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123");

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(cliente);

        await _service.Depositar(1, 100);

        cliente.Saldo.Should().Be(100);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Sacar_Deve_Alterar_Saldo_E_Salvar()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123");
        cliente.Depositar(200);

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(cliente);

        await _service.Sacar(1, 50);

        cliente.Saldo.Should().Be(150);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }

    [Fact]
    public async Task Delete_Deve_Remover_Cliente_Quando_Existir()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123");

        _repositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(cliente);

        await _service.Delete(1);

        _repositoryMock.Verify(r => r.Delete(cliente), Times.Once);
        _repositoryMock.Verify(r => r.Save(), Times.Once);
    }
}