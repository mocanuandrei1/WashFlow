using Microsoft.EntityFrameworkCore;
using WashFlow.Api.Models;

namespace WashFlow.Api.Data;

public class WashFlowDbContext : DbContext
{
    public WashFlowDbContext(DbContextOptions<WashFlowDbContext> options) : base(options) { }

    public DbSet<WashStation> Stations => Set<WashStation>();
    public DbSet<WashProgram> Programs => Set<WashProgram>();
    public DbSet<WashSession> Sessions => Set<WashSession>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<MaintenanceLog> MaintenanceLogs => Set<MaintenanceLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<WashSession>()
            .HasOne(s => s.Transaction)
            .WithOne(t => t.Session)
            .HasForeignKey<Transaction>(t => t.SessionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<WashStation>()
            .Property(s => s.Name)
            .IsRequired();

        modelBuilder.Entity<WashProgram>()
            .Property(p => p.Name)
            .IsRequired();

        modelBuilder.Entity<MaintenanceLog>()
            .Property(m => m.Title)
            .IsRequired();

        modelBuilder.Entity<MaintenanceLog>()
            .Property(m => m.Description)
            .IsRequired();
    }
}
