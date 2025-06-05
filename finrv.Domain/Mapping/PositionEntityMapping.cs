using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finrv.Domain.Mapping;

public class PositionEntityMapping : BaseEntityMapping<PositionEntity>
{
    protected override void Configure(EntityTypeBuilder<PositionEntity> builder)
    {
        builder.ToTable("posicao");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id")
            .HasColumnType("CHAR(36)");

        builder.Property(p => p.UserId)
            .HasColumnName("usuario_id")
            .HasColumnType("CHAR(36)")
            .IsRequired();

        builder.HasOne(p => p.User)
            .WithMany(u => u.Positions)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.AssetId)
            .HasColumnName("ativo_id")
            .HasColumnType("BIGINT UNSIGNED")
            .IsRequired();

        builder.HasOne(p => p.Asset)
            .WithMany(a => a.Positions)
            .HasForeignKey(p => p.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.PositionSize)
            .HasColumnName("quantidade")
            .HasColumnType("INT UNSIGNED")
            .IsRequired();

        builder.Property(p => p.AveragePrice)
            .HasColumnName("preco_medio")
            .HasColumnType("decimal(10, 2)")
            .IsRequired();

        builder.Property(p => p.ProfitAndLoss)
            .HasColumnName("lucro_ou_perda")
            .HasColumnType("decimal(10, 2)")
            .IsRequired();
    }
}
