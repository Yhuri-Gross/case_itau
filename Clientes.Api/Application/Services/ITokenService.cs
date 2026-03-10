using Clientes.Api.Domain.Entities;

namespace Clientes.Api.Application.Services;

public interface ITokenService
{
    string GerarToken(Cliente cliente);
}