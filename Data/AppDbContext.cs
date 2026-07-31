using Microsoft.EntityFrameworkCore;
using MvcCrudProject.Models;

namespace MvcCrudProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Galaxy> Galaxies { get; set; }
    public DbSet<Planet> Planets { get; set; }
    public DbSet<Continent> Continents { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Branch> Branches { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Galaxy
        modelBuilder.Entity<Galaxy>(entity =>
        {
            entity.HasKey(e => e.GalaxyId);
            entity.Property(e => e.GalaxyName).IsRequired().HasMaxLength(100);
        });

        // Planet → Galaxy
        modelBuilder.Entity<Planet>(entity =>
        {
            entity.HasKey(e => e.PlanetId);
            entity.Property(e => e.PlanetName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Galaxy)
                  .WithMany(g => g.Planets)
                  .HasForeignKey(e => e.GalaxyId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Continent → Planet
        modelBuilder.Entity<Continent>(entity =>
        {
            entity.HasKey(e => e.ContinentId);
            entity.Property(e => e.ContinentName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Planet)
                  .WithMany(p => p.Continents)
                  .HasForeignKey(e => e.PlanetId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Region → Continent
        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId);
            entity.Property(e => e.RegionName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Continent)
                  .WithMany(c => c.Regions)
                  .HasForeignKey(e => e.ContinentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Country → Region
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId);
            entity.Property(e => e.CountryName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Region)
                  .WithMany(r => r.Countries)
                  .HasForeignKey(e => e.RegionId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // City → Country
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId);
            entity.Property(e => e.CityName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Country)
                  .WithMany(c => c.Cities)
                  .HasForeignKey(e => e.CountryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Area → City
        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.AreaId);
            entity.Property(e => e.AreaName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.City)
                  .WithMany(c => c.Areas)
                  .HasForeignKey(e => e.CityId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Company
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId);
            entity.Property(e => e.CompanyName).IsRequired().HasMaxLength(100);
        });

        // Branch → Area + Company
        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId);
            entity.Property(e => e.BranchName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Area)
                  .WithMany(a => a.Branches)
                  .HasForeignKey(e => e.AreaId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Company)
                  .WithMany(c => c.Branches)
                  .HasForeignKey(e => e.CompanyId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Department → Branch
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);
            entity.Property(e => e.DepartmentName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Branch)
                  .WithMany(b => b.Departments)
                  .HasForeignKey(e => e.BranchId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Section → Department
        modelBuilder.Entity<Section>(entity =>
        {
            entity.HasKey(e => e.SectionId);
            entity.Property(e => e.SectionName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Department)
                  .WithMany(d => d.Sections)
                  .HasForeignKey(e => e.DepartmentId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Employee → Section
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
            entity.Property(e => e.EmployeeName).IsRequired().HasMaxLength(100);
            entity.HasOne(e => e.Section)
                  .WithMany(s => s.Employees)
                  .HasForeignKey(e => e.SectionId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
