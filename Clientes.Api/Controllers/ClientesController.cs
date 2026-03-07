using Clientes.Api.Application.DTOs;
using Clientes.Api.Application.Services;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var cliente = await _service.GetById(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClienteCreateDto dto)
    {
        var id = await _service.Create(dto);

        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ClienteUpdateDto dto)
    {
        await _service.Update(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.Delete(id);

        return NoContent();
    }

    [HttpPost("{id}/depositar")]
    public async Task<IActionResult> Depositar(int id, OperacaoSaldoDto dto)
    {
        await _service.Depositar(id, dto.Valor);

        return Ok();
    }

    [HttpPost("{id}/sacar")]
    public async Task<IActionResult> Sacar(int id, OperacaoSaldoDto dto)
    {
        await _service.Sacar(id, dto.Valor);

        return Ok();
    }
}