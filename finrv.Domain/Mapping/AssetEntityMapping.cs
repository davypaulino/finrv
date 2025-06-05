using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finrv.Domain.Mapping;

public class AssetEntityMapping : BaseEntityMapping<AssetEntity>
{
    protected override void Configure(EntityTypeBuilder<AssetEntity> builder)
    {
        builder.ToTable("ativo");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnType("serial")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Ticker)
            .HasColumnName("codigo")
            .HasColumnType("varchar(10)")
            .IsRequired();

        builder.Property(a => a.Name)
            .HasColumnName("nome")
            .HasColumnType("varchar(150)")
            .IsRequired();
    }
}
