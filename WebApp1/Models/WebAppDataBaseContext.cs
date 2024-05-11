using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApp1.Models;

public partial class WebAppDataBaseContext : DbContext
{
    public WebAppDataBaseContext()
    {
    }

    public WebAppDataBaseContext(DbContextOptions<WebAppDataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = WebApplication.CreateBuilder();
        var connectionString = builder.Configuration.GetConnectionString ("MyConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasIndex(e => e.RoomId, "IX_Reservations_RoomId");

            entity.HasOne(d => d.Room).WithMany(p => p.Reservations).HasForeignKey(d => d.RoomId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
