using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using todolist.Models;

namespace todolist.Contexts;

public partial class todolistContext : DbContext
{
    public todolistContext()
    {
    }

    public todolistContext(DbContextOptions<todolistContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Prioridade> Prioridades { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tarefa> Tarefas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=NOTE17-S21\\SQLSERVER; initial catalog=todolist; User Id = sa; Pwd = Senai@134; TrustServerCertificate=true;");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Prioridade>(entity =>
        {
            entity.HasKey(e => e.IdPrioridade).HasName("PK__Priorida__3A85138FBB8D5AD0");

            entity.ToTable("Prioridade");

            entity.Property(e => e.Titulo).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__Status__B450643A2CDCD749");

            entity.ToTable("Status");

            entity.Property(e => e.Status1)
                .HasMaxLength(50)
                .HasColumnName("Status");
        });

        modelBuilder.Entity<Tarefa>(entity =>
        {
            entity.HasKey(e => e.IdTarefa).HasName("PK__Tarefa__61D038D70F11FDE9");

            entity.ToTable("Tarefa");

            entity.Property(e => e.Setor).HasMaxLength(50);

            entity.HasOne(d => d.IdPrioridadeNavigation).WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.IdPrioridade)
                .HasConstraintName("FK__Tarefa__IdPriori__5165187F");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK__Tarefa__IdStatus__52593CB8");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Tarefas)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Tarefa__IdUsuari__5070F446");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF97E3548BD5");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D10534BDECDE15").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nome).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
