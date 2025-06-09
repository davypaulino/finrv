using finrv.Domain.Entities;
using finrv.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace finrv.Infra;

public class InvestimentDbContext : DbContext
{
    private InvestimentDbContext() { }

    private readonly ILogger<InvestimentDbContext> _logger;
    private readonly RequestInfo _requestInfo;
    public InvestimentDbContext(DbContextOptions<InvestimentDbContext> options,
        RequestInfo requestInfo, ILogger<InvestimentDbContext> logger) : base(options) {
        _requestInfo = requestInfo;
        _logger = logger;
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
        var currentUser = "SYSTEM_WORKER_SERVICE";
        if (_requestInfo.UserId is not null)
            currentUser = _requestInfo.UserId;
        var currentDateTime = DateTime.Now;
        
        foreach (var entry in  ChangeTracker.Entries<BaseEntity>())
        {
            var auditableEntity = entry.Entity;
            if (auditableEntity is QuotationEntity)
            {
                if (auditableEntity.CreatedAt is not null)
                    currentDateTime = (DateTime)auditableEntity.CreatedAt;
                if (auditableEntity.UpdatedAt is not null)
                    currentDateTime = (DateTime)auditableEntity.UpdatedAt;
                if  (auditableEntity.CreatedBy is not null)
                    currentUser = auditableEntity.CreatedBy;
                if (auditableEntity.UpdatedBy is not null)
                    currentUser = auditableEntity.UpdatedBy;
            }
            
            switch (entry.State)
            {
                case EntityState.Added:
                    if (auditableEntity.CreatedAt == null)
                    {
                        auditableEntity.CreatedAt = currentDateTime;
                    }
                    if (string.IsNullOrWhiteSpace(auditableEntity.CreatedBy))
                    {
                        auditableEntity.CreatedBy = currentUser;
                    }
                    auditableEntity.UpdatedAt = currentDateTime;
                    auditableEntity.UpdatedBy = currentUser;
                    break;

                case EntityState.Modified:
                    auditableEntity.UpdatedAt = currentDateTime;
                    auditableEntity.UpdatedBy = currentUser;
                    entry.Property(nameof(BaseEntity.CreatedAt)).IsModified = false;
                    entry.Property(nameof(BaseEntity.CreatedBy)).IsModified = false;
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
