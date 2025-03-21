﻿using Microsoft.EntityFrameworkCore;
using ShelekhovResult.DataBase.Models;

namespace ShelekhovResult.DataBase;

/// <summary>
/// Контекст базы данных
/// </summary>
public class TradeDbContext : DbContext
{
    public TradeDbContext(DbContextOptions<TradeDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Data> Data { get; set; }

    protected  override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<User>()
            .HasKey(u => u.Id);

        builder.Entity<User>()
            .Property(u => u.Id)
            .HasDefaultValueSql("NEWID()");
        
        builder.Entity<Data>()
            .Property(u => u.Id)
            .HasDefaultValueSql("NEWID()");

        builder.Entity<Data>()
            .HasKey(d => d.Id);
        
        builder.Entity<Data>()
            .HasOne(d => d.User)
            .WithMany(u => u.Data)
            .HasForeignKey(d => d.UserId) 
            .OnDelete(DeleteBehavior.Cascade);
    }
}