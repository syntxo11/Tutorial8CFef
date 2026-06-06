using Microsoft.EntityFrameworkCore;
using Tutorial8CFef.Entities;

namespace Tutorial8CFef.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Pc> Pcs { get; set; }
    public DbSet<Component> Components { get; set; }
    public DbSet<PcComponent> PcComponents { get; set; }
    public DbSet<ComponentType> ComponentTypes { get; set; }
    public DbSet<ComponentManufacturer> ComponentManufacturers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pc>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<Component>(e =>
        {
            e.HasKey(x => x.Code);
            e.Property(x => x.Code).HasMaxLength(10);
            e.Property(x => x.Name).HasMaxLength(300).IsRequired();
            e.Property(x => x.Description).IsRequired();

            e.HasOne(x => x.ComponentManufacturer)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.ComponentManufacturerId);

            e.HasOne(x => x.ComponentType)
                .WithMany(x => x.Components)
                .HasForeignKey(x => x.ComponentTypeId);
        });

        modelBuilder.Entity<ComponentManufacturer>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Abbreviation).HasMaxLength(30).IsRequired();
            e.Property(x => x.FullName).HasMaxLength(300).IsRequired();
        });

        modelBuilder.Entity<ComponentType>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Abbreviation).HasMaxLength(30).IsRequired();
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
        });

        modelBuilder.Entity<PcComponent>(e =>
        {
            e.HasKey(x => new { x.PcId, x.ComponentCode });

            e.HasOne(x => x.Pc)
                .WithMany(x => x.PcComponents)
                .HasForeignKey(x => x.PcId);

            e.HasOne(x => x.Component)
                .WithMany(x => x.PcComponents)
                .HasForeignKey(x => x.ComponentCode);
        });

        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Abbreviation = "CPU", Name = "Processor" },
            new ComponentType { Id = 2, Abbreviation = "GPU", Name = "Graphics Card" },
            new ComponentType { Id = 3, Abbreviation = "RAM", Name = "Memory" }
        );

        modelBuilder.Entity<ComponentManufacturer>().HasData(
            new ComponentManufacturer { Id = 1, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateOnly(1969, 5, 1) },
            new ComponentManufacturer { Id = 2, Abbreviation = "NV", FullName = "NVIDIA Corporation", FoundationDate = new DateOnly(1993, 4, 5) },
            new ComponentManufacturer { Id = 3, Abbreviation = "COR", FullName = "Corsair Gaming Inc.", FoundationDate = new DateOnly(1994, 1, 1) }
        );

        modelBuilder.Entity<Component>().HasData(
            new Component { Code = "CPU0000001", Name = "Ryzen 7 7800X3D", Description = "8-core gaming processor", ComponentManufacturerId = 1, ComponentTypeId = 1 },
            new Component { Code = "GPU0000001", Name = "RTX 4080 Super", Description = "High-end gaming graphics card", ComponentManufacturerId = 2, ComponentTypeId = 2 },
            new Component { Code = "RAM0000001", Name = "Corsair Vengeance DDR5 16GB", Description = "DDR5 RAM module 16GB", ComponentManufacturerId = 3, ComponentTypeId = 3 }
        );

        modelBuilder.Entity<Pc>().HasData(
            new Pc { Id = 1, Name = "Gaming Beast X", Weight = 12.5f, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
            new Pc { Id = 2, Name = "Office Mini Pro", Weight = 4.2f, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
            new Pc { Id = 3, Name = "Budget Home PC", Weight = 6.3f, Warranty = 12, CreatedAt = new DateTime(2026, 3, 10, 10, 0, 0), Stock = 8 }
        );

        modelBuilder.Entity<PcComponent>().HasData(
            new PcComponent { PcId = 1, ComponentCode = "CPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "GPU0000001", Amount = 1 },
            new PcComponent { PcId = 1, ComponentCode = "RAM0000001", Amount = 2 }
        );
    }
}