using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TicketSeller.DBModel;

public sealed class TicketDbContext : DbContext
{
    public TicketDbContext(DbContextOptions<TicketDbContext> options) : base(options)
    {
        // Database.EnsureDeleted();
        // Database.EnsureCreated();
        
        Tickets.Find(1).Tag = Tags.Find(2);
        Tickets.Find(2).Tag = Tags.Find(3);
        SaveChanges();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(GetEmployees());
        modelBuilder.Entity<Tag>().HasData(GetTags());
        modelBuilder.Entity<Ticket>().HasData(GetTickets());
        base.OnModelCreating(modelBuilder);
    }

    private static IEnumerable<User> GetEmployees()
    {
        return new List<User>
        {
            new()
            {
                // ReSharper disable StringLiteralTypo
                Id = "wJVRrOnBihskSkY24viYYKA2ftFjOIyna4c0Rve2PzhZ4tbgDJ5kU94ZT4VZwCfLuI5QaJUgS4qqANzO00GBIvM95ubF3hBbghSa5XTseINoB5mvXva8XSNEswlpvQ53Y1yQ6KbNwiNUdHU3xb5fSMyqeIic1KidUVNBZbtfAhD5d4nWKgJGj5Pn5OCn4US1u8o1xvHDPSPW3OzKnevfaFKDoNRLwEItTvCqIhsHzJKm0DvwN7Z1xYqIqvORBmMwEzFbtM75y6EqRty5Hw8Bdn8tc8AO43mBMj9Ynn8p2EA8wltF7zp6aeWHDqSLIE04vyes6CMPLF6OW3JNkALddW07j5IPAHh5qekIeABs9kPCKy6xzBuCp6O75rVCII8YasOgWB4lZbD8RTRXhPZH2CCYnG31rMLr2lIVXI0oFOU2TqdcTfJaHPOVBMYhD4AxBM", 
                UserName = "Admin", 
                HashPassword = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("Admin")),
                Role = UsersRole.Admin
            },
        };
    }
    
    private static IEnumerable<Ticket> GetTickets()
    {
        return new List<Ticket>
        {
            new()
            {
                Id = 1,
                TicketName = "Концерт AC/DC",
                DateOfConcert = DateTime.Now,
                ConcertPlace = "Дрезден"
            },
            new()
            {
                Id = 2,
                TicketName = "Концерт Эминема",
                DateOfConcert = DateTime.Now,
                ConcertPlace = "Нью Йорк"
            },
        };
    }
    
    private static IEnumerable<Tag> GetTags()
    {
        return new List<Tag>
        {
            new()
            {
                Id = 1,
                TagName = "Поп"
            },
            new()
            {
                Id = 2,
                TagName = "Рок"
            },
            new()
            {
                Id = 3,
                TagName = "Рэп"
            },
            new()
            {
                Id = 4,
                TagName = "Джаз"
            },
        };
    }

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Ticket> Tickets { get; set; } = null!;

    public DbSet<Tag> Tags { get; set; } = null!;
}