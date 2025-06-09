using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace finrv.Infra;

public class InvestimentDbContext : DbContext
{
    private InvestimentDbContext() { }

    public InvestimentDbContext(DbContextOptions<InvestimentDbContext> options) : base(options) {
        
    }

    public virtual DbSet<UserEntity> User { get; set; }
    public virtual DbSet<AssetEntity> Asset { get; set; }
    public virtual DbSet<QuotationEntity> Quotation { get; set; }
    public virtual DbSet<TransactionEntity> Transaction { get; set; }
    public virtual DbSet<PositionEntity> Position { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvestimentDbContext).Assembly);
    }
    
    private void OnBeforeSaving()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var currentUser = "SYSTEM_WORKER_SERVICE";

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.UpdatedBy = currentUser;
                    break;

                case EntityState.Modified:
                    if (entry.OriginalValues["Id"] == null)
                        entry.Entity.CreatedBy = currentUser;
                    entry.Entity.UpdatedBy = currentUser;
                    break;
            }
        }
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
