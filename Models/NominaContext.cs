using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Nomina.Models;

public partial class NominaContext : DbContext
{
    public NominaContext()
    {
    }

    public NominaContext(DbContextOptions<NominaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Empleado> Empleado { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySQL("server=localhost;password=root;user=root;database=nomina");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.SueldoMaximo).HasPrecision(6);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empleado");

            entity.HasIndex(e => e.IdCategoria, "fk_empleadocategoria");

            entity.Property(e => e.Activo).HasDefaultValueSql("'0'");
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.Sueldo).HasPrecision(10);

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Empleado)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_empleadocategoria");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
