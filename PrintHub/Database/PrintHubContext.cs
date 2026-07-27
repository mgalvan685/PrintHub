using Microsoft.EntityFrameworkCore;
using PrintHub.Database.Models;
using System.Reflection.Emit;

namespace PrintHub.Database;

public class PrintHubContext : DbContext
{
    public PrintHubContext(DbContextOptions<PrintHubContext> options)
        : base(options) { }

    public DbSet<Printer> Printers => Set<Printer>();
    public DbSet<Filament> Filaments => Set<Filament>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectFilament> ProjectFilaments => Set<ProjectFilament>();
    public DbSet<ProjectMaterial> ProjectMaterials => Set<ProjectMaterial>();
    public DbSet<PriceModifier> PriceModifiers => Set<PriceModifier>();
    public DbSet<PrintEvent> PrintEvents => Set<PrintEvent>();
    public DbSet<CostBreakdown> CostBreakdowns => Set<CostBreakdown>();
    public DbSet<InventoryTransaction> InventoryTransactions => Set<InventoryTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Example: ProjectFilament relationships
        modelBuilder.Entity<ProjectFilament>(entity =>
        {
            entity.HasOne(pf => pf.Project)
                  .WithMany(p => p.ProjectFilaments)
                  .HasForeignKey(pf => pf.Project_ID)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pf => pf.Filament)
                  .WithMany(f => f.ProjectFilaments)
                  .HasForeignKey(pf => pf.Filament_ID)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProjectMaterial>(entity =>
        {
            entity.HasOne(pm => pm.Project)
                  .WithMany(p => p.ProjectMaterials)
                  .HasForeignKey(pm => pm.Project_ID)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pm => pm.Material)
                  .WithMany(m => m.ProjectMaterials)
                  .HasForeignKey(pm => pm.Material_ID)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created_On = now;
                entry.Entity.Created_By ??= "system";
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Updated_On = now;
            }
        }

        return base.SaveChanges();
    }
}
