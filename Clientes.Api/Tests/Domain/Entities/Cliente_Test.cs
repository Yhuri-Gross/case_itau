using Clientes.Api.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Clientes.Api.Tests.Domain;

public class ClienteTests
{
    [Fact]
    public void Deve_Criar_Cliente_Com_Saldo_Inicial_Zero()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        cliente.Nome.Should().Be("Maria");
        cliente.Email.Should().Be("maria@email.com");
        cliente.Saldo.Should().Be(0);
        cliente.Role.Should().Be("User");
    }

    [Fact]
    public void Deve_Atualizar_Nome_E_Email()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        cliente.Atualizar("Joana", "joana@email.com");

        cliente.Nome.Should().Be("Joana");
        cliente.Email.Should().Be("joana@email.com");
    }

    [Fact]
    public void Deve_Depositar_Valor_Valido()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        cliente.Depositar(100);

        cliente.Saldo.Should().Be(100);
    }

    [Fact]
    public void Nao_Deve_Depositar_Valor_Menor_Ou_Igual_A_Zero()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        var act = () => cliente.Depositar(0);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Valor de depósito inválido");
    }

    [Fact]
    public void Deve_Sacar_Valor_Valido()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");
        cliente.Depositar(200);

        cliente.Sacar(50);

        cliente.Saldo.Should().Be(150);
    }

    [Fact]
    public void Nao_Deve_Sacar_Com_Saldo_Insuficiente()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");
        cliente.Depositar(50);

        var act = () => cliente.Sacar(100);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Saldo insuficiente");
    }

    [Fact]
    public void Nao_Deve_Sacar_Valor_Menor_Ou_Igual_A_Zero()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        var act = () => cliente.Sacar(0);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Valor de saque inválido");
    }

    [Fact]
    public void Deve_Validar_Senha_Correta()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        var resultado = cliente.ValidarSenha("123456");

        resultado.Should().BeTrue();
    }

    [Fact]
    public void Deve_Invalidar_Senha_Incorreta()
    {
        var cliente = new Cliente("Maria", "maria@email.com", "123456");

        var resultado = cliente.ValidarSenha("outra-senha");

        resultado.Should().BeFalse();
    }
}