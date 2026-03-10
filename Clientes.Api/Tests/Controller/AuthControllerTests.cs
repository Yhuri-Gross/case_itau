using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Clientes.Api.Controllers;
using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Clientes.Api.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IClienteRepository> _repoMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _repoMock = new Mock<IClienteRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _controller = new AuthController(_repoMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Login_Deve_Retornar_Unauthorized_Quando_Cliente_Nao_Existir()
    {
        var dto = new LoginDto
        {
            Email = "naoexiste@email.com",
            Senha = "123456"
        };

        _repoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync((Cliente?)null);

        var resultado = await _controller.Login(dto);

        resultado.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_Deve_Retornar_Unauthorized_Quando_Senha_For_Invalida()
    {
        var dto = new LoginDto
        {
            Email = "maria@email.com",
            Senha = "senha-errada"
        };

        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        _repoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync(cliente);

        var resultado = await _controller.Login(dto);

        resultado.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Login_Deve_Retornar_Ok_Quando_Credenciais_Forem_Validas()
    {
        var dto = new LoginDto
        {
            Email = "maria@email.com",
            Senha = "123456"
        };

        var cliente = new Cliente("Maria", "maria@email.com", "123456", "Admin");

        _repoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync(cliente);
        _tokenServiceMock.Setup(t => t.GerarToken(cliente)).Returns("token-fake");

        var resultado = await _controller.Login(dto);

        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().NotBeNull();
    }
}