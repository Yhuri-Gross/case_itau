using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Clientes.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Clientes.Api.Tests.Controllers;

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
    public async Task GetAll_Deve_Retornar_Ok_Com_Lista()
    {
        var lista = new List<ClienteResponseDto>
        {
            new() { Id = 1, Nome = "Maria", Email = "maria@email.com", Saldo = 100 }
        };

        _serviceMock.Setup(s => s.GetAll()).ReturnsAsync(lista);

        var resultado = await _controller.GetAll();

        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(lista);
    }

    [Fact]
    public async Task GetById_Deve_Retornar_Forbid_Quando_User_Tentar_Acessar_Outro_Id()
    {
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = GerarUsuarioClaims(1, "User")
            }
        };

        var resultado = await _controller.GetById(2);

        resultado.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async Task GetById_Deve_Retornar_Ok_Quando_User_Acessar_Proprio_Id()
    {
        var cliente = new ClienteResponseDto
        {
            Id = 1,
            Nome = "Maria",
            Email = "maria@email.com",
            Saldo = 100
        };

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = GerarUsuarioClaims(1, "User")
            }
        };

        _serviceMock.Setup(s => s.GetById(1)).ReturnsAsync(cliente);

        var resultado = await _controller.GetById(1);

        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(cliente);
    }

    [Fact]
    public async Task GetById_Deve_Retornar_Ok_Quando_Admin_Acessar_Qualquer_Id()
    {
        var cliente = new ClienteResponseDto
        {
            Id = 2,
            Nome = "Joao",
            Email = "joao@email.com",
            Saldo = 200
        };

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = GerarUsuarioClaims(1, "Admin")
            }
        };

        _serviceMock.Setup(s => s.GetById(2)).ReturnsAsync(cliente);

        var resultado = await _controller.GetById(2);

        var okResult = resultado.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(cliente);
    }

    private static ClaimsPrincipal GerarUsuarioClaims(int id, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }
}