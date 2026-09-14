using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Projeto_BolosJacquin.Models;

namespace Projeto_BolosJacquin.BdContext;

public partial class JacquinContext : DbContext
{
    public JacquinContext()
    {
    }

    public JacquinContext(DbContextOptions<JacquinContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Avaliacoes> Avaliacoes { get; set; }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Produtos> Produtos { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //        => optionsBuilder.UseSqlServer("Server=D11S20-1252882\\MSSQLSERVER3;Database=Projetos_Bolos_Jacquin;User Id=sa;Password=Senai@134;TrustServerCertificate=True;");
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Avaliacoes>(entity =>
        {
            entity.HasKey(e => e.AvaliacaoId).HasName("PK__Avaliaco__FC95FF38B2ADF44F");

            entity.Property(e => e.AvaliacaoId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Produto).WithMany(p => p.Avaliacoes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Avaliacoe__Produ__6B24EA82");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Avaliacoes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Avaliacoe__Usuar__6A30C649");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1C5B6F9DAF3");

            entity.Property(e => e.CategoriaId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<Produtos>(entity =>
        {
            entity.HasKey(e => e.ProdutoId).HasName("PK__Produtos__9C8800C394ADFF9E");

            entity.Property(e => e.ProdutoId).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Produtos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Produtos__Catego__656C112C");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798CBA0558E");

            entity.Property(e => e.UsuarioId).HasDefaultValueSql("(newid())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
