using Clientes.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clientes.Api.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Cliente>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nome)
                .IsRequired();

            entity.Property(x => x.Email)
                .IsRequired();

            entity.Property(x => x.Senha)
                .IsRequired();

            entity.Property(x => x.Role)
                .IsRequired()
                .HasDefaultValue("User");

            entity.Property(x => x.Saldo)
                .HasColumnType("decimal(18,2)");

            entity.HasIndex(x => x.Email)
                .IsUnique();
        });
    }
}