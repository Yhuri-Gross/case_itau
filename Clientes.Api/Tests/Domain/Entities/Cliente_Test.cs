using Clientes.Api.Domain.Entities;
using Xunit;

namespace Clientes.Tests.Domain.Entities;

public class ClienteTests
{
    [Fact]
    public void Constructor_DeveCriarClienteComSaldoZero()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        Assert.Equal("Yhuri", cliente.Nome);
        Assert.Equal("yhuri@email.com", cliente.Email);
        Assert.Equal(0, cliente.Saldo);
    }

    [Fact]
    public void Atualizar_DeveAlterarNomeEEmail()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        cliente.Atualizar("Novo Nome", "novo@email.com");

        Assert.Equal("Novo Nome", cliente.Nome);
        Assert.Equal("novo@email.com", cliente.Email);
    }

    [Fact]
    public void Depositar_DeveAdicionarSaldo()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        cliente.Depositar(100);

        Assert.Equal(100, cliente.Saldo);
    }

    [Fact]
    public void Depositar_DeveLancarExcecao_QuandoValorInvalido()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        Assert.Throws<ArgumentException>(() => cliente.Depositar(0));
        Assert.Throws<ArgumentException>(() => cliente.Depositar(-10));
    }

    [Fact]
    public void Sacar_DeveDiminuirSaldo()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        cliente.Depositar(100);
        cliente.Sacar(40);

        Assert.Equal(60, cliente.Saldo);
    }

    [Fact]
    public void Sacar_DeveLancarExcecao_QuandoValorInvalido()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        cliente.Depositar(100);

        Assert.Throws<ArgumentException>(() => cliente.Sacar(0));
        Assert.Throws<ArgumentException>(() => cliente.Sacar(-10));
    }

    [Fact]
    public void Sacar_DeveLancarExcecao_QuandoSaldoInsuficiente()
    {
        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        cliente.Depositar(50);

        Assert.Throws<InvalidOperationException>(() => cliente.Sacar(100));
    }
}