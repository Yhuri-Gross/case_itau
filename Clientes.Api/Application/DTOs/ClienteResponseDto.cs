namespace Clientes.Api.Application.DTOs;

public class ClienteResponseDto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public decimal Saldo { get; set; }
}