using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace labsapp.Models;

public partial class ToDoDatabaseContext : DbContext
{
    public ToDoDatabaseContext()
    {
    }

    public ToDoDatabaseContext(DbContextOptions<ToDoDatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblTodo> TblTodos { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = WebApplication.CreateBuilder();
        var connectionString = builder.Configuration.GetConnectionString ("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblTodo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_todo__3213E83FDC3833B7");

            entity.ToTable("tbl_todo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("endDate");
            entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
