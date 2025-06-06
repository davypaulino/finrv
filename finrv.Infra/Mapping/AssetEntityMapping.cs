using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finrv.Infra.Mapping;

public class AssetEntityMapping : BaseEntityMapping<AssetEntity>
{
    protected override void Configure(EntityTypeBuilder<AssetEntity> builder)
    {
        builder.ToTable("ativo");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("id")
            .HasColumnType("BIGINT UNSIGNED")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Ticker)
            .HasColumnName("codigo")
            .HasColumnType("varchar(8)")
            .IsRequired();

        builder.Property(a => a.Name)
            .HasColumnName("nome")
            .HasColumnType("varchar(150)")
            .IsRequired();
    }
}
