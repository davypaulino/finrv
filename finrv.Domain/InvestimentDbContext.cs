using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace finrv.Domain;

public class InvestimentDbContext : DbContext
{
    private InvestimentDbContext() { }

    public InvestimentDbContext(DbContextOptions<InvestimentDbContext> options) : base(options) {
        Console.WriteLine("Hello!");
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
}
