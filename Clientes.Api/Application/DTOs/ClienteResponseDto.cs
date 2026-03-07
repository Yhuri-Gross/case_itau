namespace Clientes.Api.Application.DTOs;

public class ClienteResponseDto
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public string Email { get; set; }

    public decimal Saldo { get; set; }
}