using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Clientes.Api.Controllers;

[ApiController]
[Route("clientes")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;

    public ClientesController(IClienteService service)
    {
        _service = service;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var cliente = await _service.GetById(userId);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)!.Value;

        if (role != "Admin" && userId != id)
            return Forbid();

        var cliente = await _service.GetById(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDto dto)
    {
        var id = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)!.Value;

        if (role != "Admin" && userId != id)
            return Forbid();

        await _service.Update(id, dto);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)!.Value;

        if (role != "Admin" && userId != id)
            return Forbid();

        await _service.Delete(id);
        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/depositar")]
    public async Task<IActionResult> Depositar(int id, [FromBody] OperacaoSaldoDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)!.Value;

        if (role != "Admin" && userId != id)
            return Forbid();

        await _service.Depositar(id, dto.Valor);
        return Ok();
    }

    [Authorize]
    [HttpPost("{id}/sacar")]
    public async Task<IActionResult> Sacar(int id, [FromBody] OperacaoSaldoDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var role = User.FindFirst(ClaimTypes.Role)!.Value;

        if (role != "Admin" && userId != id)
            return Forbid();

        await _service.Sacar(id, dto.Valor);
        return Ok();
    }
}