using Microsoft.EntityFrameworkCore;
using SegurosAuto.Api.Models;

namespace SegurosAuto.Api.Data;

/// <summary>
/// Contexto de Entity Framework Core: puente entre el código y SQL Server.
/// Define tablas, llaves, tipos de dato, índices únicos y datos iniciales.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>Tabla Clientes.</summary>
    public DbSet<Cliente> Clientes => Set<Cliente>();

    /// <summary>Tabla Vehiculos.</summary>
    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();

    /// <summary>Tabla Coberturas (catálogo).</summary>
    public DbSet<Cobertura> Coberturas => Set<Cobertura>();

    /// <summary>Tabla Polizas.</summary>
    public DbSet<Poliza> Polizas => Set<Poliza>();

    /// <summary>Tabla intermedia PolizaCoberturas.</summary>
    public DbSet<PolizaCobertura> PolizaCoberturas => Set<PolizaCobertura>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(e =>
        {
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            e.Property(x => x.Identificacion).IsRequired().HasMaxLength(30);
            e.Property(x => x.Correo).IsRequired().HasMaxLength(150);
            e.HasIndex(x => x.Identificacion).IsUnique();
        });

        modelBuilder.Entity<Vehiculo>(e =>
        {
            e.Property(x => x.Placa).IsRequired().HasMaxLength(15);
            e.Property(x => x.Marca).IsRequired().HasMaxLength(50);
            e.Property(x => x.Modelo).IsRequired().HasMaxLength(50);
            e.Property(x => x.ValorComercial).HasColumnType("decimal(18,2)");
            e.HasIndex(x => x.Placa).IsUnique();
        });

        modelBuilder.Entity<Cobertura>(e =>
        {
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            e.Property(x => x.MontoCobertura).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Poliza>(e =>
        {
            e.Property(x => x.NumeroPoliza).IsRequired().HasMaxLength(30);
            e.Property(x => x.SumaAsegurada).HasColumnType("decimal(18,2)");
            e.Property(x => x.PrimaTotal).HasColumnType("decimal(18,2)");
            e.HasIndex(x => x.NumeroPoliza).IsUnique();

            // Restrict: no se puede borrar un cliente/vehículo que tenga pólizas
            e.HasOne(x => x.Cliente)
                .WithMany(c => c.Polizas)
                .HasForeignKey(x => x.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.Vehiculo)
                .WithMany(v => v.Polizas)
                .HasForeignKey(x => x.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PolizaCobertura>(e =>
        {
            e.HasKey(x => new { x.PolizaId, x.CoberturaId }); // llave compuesta
            e.Property(x => x.MontoCobertura).HasColumnType("decimal(18,2)");

            e.HasOne(x => x.Poliza)
                .WithMany(p => p.Coberturas)
                .HasForeignKey(x => x.PolizaId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Cobertura)
                .WithMany(c => c.PolizaCoberturas)
                .HasForeignKey(x => x.CoberturaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Datos iniciales (catálogos)
        modelBuilder.Entity<Cobertura>().HasData(
            new Cobertura { Id = 1, Nombre = "Responsabilidad Civil", MontoCobertura = 1500m },
            new Cobertura { Id = 2, Nombre = "Choque", MontoCobertura = 2500m },
            new Cobertura { Id = 3, Nombre = "Robo", MontoCobertura = 2000m },
            new Cobertura { Id = 4, Nombre = "Incendio", MontoCobertura = 1000m },
            new Cobertura { Id = 5, Nombre = "Asistencia Vial", MontoCobertura = 300m }
        );

        modelBuilder.Entity<Cliente>().HasData(
            new Cliente { Id = 1, Nombre = "Juan Pérez", Identificacion = "001-010190-0001A", Correo = "juan.perez@correo.com" },
            new Cliente { Id = 2, Nombre = "María López", Identificacion = "001-150285-0002B", Correo = "maria.lopez@correo.com" }
        );
    }
}