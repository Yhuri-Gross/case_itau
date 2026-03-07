namespace Clientes.Api.Domain.Entities;

public class Cliente
{
    public int Id { get; private set; }

    public string Nome { get; private set; }

    public string Email { get; private set; }

    public decimal Saldo { get; private set; }

    public Cliente(string nome, string email)
    {
        Nome = nome;
        Email = email;
        Saldo = 0;
    }

    public void Atualizar(string nome, string email)
    {
        Nome = nome;
        Email = email;
    }

    public void Depositar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor de depósito inválido");

        Saldo += valor;
    }

    public void Sacar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor de saque inválido");

        if (Saldo < valor)
            throw new InvalidOperationException("Saldo insuficiente");

        Saldo -= valor;
    }
}