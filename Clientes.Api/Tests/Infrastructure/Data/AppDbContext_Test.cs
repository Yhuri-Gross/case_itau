using Clientes.Api.Domain.Entities;
using Clientes.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Clientes.Tests.Infrastructure.Data;

public class AppDbContextTests
{
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void DbSetClientes_DeveExistir()
    {
        using var context = CreateContext();

        Assert.NotNull(context.Clientes);
    }

    [Fact]
    public void DeveAdicionarClienteNoContexto()
    {
        using var context = CreateContext();

        var cliente = new Cliente("Yhuri", "yhuri@email.com");

        context.Clientes.Add(cliente);
        context.SaveChanges();

        Assert.Single(context.Clientes);
    }

    [Fact]
    public void DevePersistirClienteNoBancoEmMemoria()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("db_test")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var cliente = new Cliente("Yhuri", "yhuri@email.com");
            context.Clientes.Add(cliente);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(options))
        {
            var cliente = context.Clientes.FirstOrDefault();

            Assert.NotNull(cliente);
            Assert.Equal("Yhuri", cliente.Nome);
            Assert.Equal("yhuri@email.com", cliente.Email);
        }
    }

    [Fact]
    public void Model_DeveTerChavePrimariaConfigurada()
    {
        using var context = CreateContext();

        var entity = context.Model.FindEntityType(typeof(Cliente));
        var key = entity.FindPrimaryKey();

        Assert.NotNull(key);
        Assert.Single(key.Properties);
        Assert.Equal("Id", key.Properties.First().Name);
    }

    [Fact]
    public void Model_Email_DeveSerUnico()
    {
        using var context = CreateContext();

        var entity = context.Model.FindEntityType(typeof(Cliente));
        var index = entity.GetIndexes().FirstOrDefault(i => i.Properties.Any(p => p.Name == "Email"));

        Assert.NotNull(index);
        Assert.True(index.IsUnique);
    }
}