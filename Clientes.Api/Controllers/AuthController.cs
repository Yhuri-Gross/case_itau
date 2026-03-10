using Clientes.Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;

namespace Clientes.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IClienteRepository _repo;
    private readonly ITokenService _tokenService;

    public AuthController(IClienteRepository repo, ITokenService tokenService)
    {
        _repo = repo;
        _tokenService = tokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var cliente = await _repo.GetByEmail(dto.Email);

        if (cliente == null || !cliente.ValidarSenha(dto.Senha))
            return Unauthorized(new { message = "Credenciais inválidas" });

        var token = _tokenService.GerarToken(cliente);

        return Ok(new LoginResponseDto
        {
            Token = token,
            Id = cliente.Id,
            Nome = cliente.Nome,
            Email = cliente.Email,
            Role = cliente.Role
        });
    }
}