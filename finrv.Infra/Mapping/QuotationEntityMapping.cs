using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finrv.Infra.Mapping;

public class QuotationEntityMapping : BaseEntityMapping<QuotationEntity>
{
    protected override void Configure(EntityTypeBuilder<QuotationEntity> builder)
    {
        builder.ToTable("cotacao");

        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .HasColumnName("id")
            .HasColumnType("BIGINT UNSIGNED")
            .ValueGeneratedOnAdd();

        builder.Property(q => q.AssetId)
            .HasColumnName("ativo_id")
            .HasColumnType("BIGINT UNSIGNED")
            .IsRequired();

        builder.HasOne(q => q.Asset)
            .WithMany(a => a.Quotations)
            .HasForeignKey(q => q.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(q => q.Price)
            .HasColumnName("preco_unitario")
            .HasColumnType("decimal(10,2)")
            .IsRequired();
    }
}
