using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finrv.Infra.Mapping;

public class TransactionEntityMapping : BaseEntityMapping<TransactionEntity>
{
    protected override void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.ToTable("operacoes");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id")
            .HasColumnType("BIGINT UNSIGNED")
            .ValueGeneratedOnAdd();

        builder.Property(t => t.UserId)
            .HasColumnName("usuario_id")
            .HasColumnType("CHAR(36)")
            .IsRequired();

        builder.HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.AssetId)
            .HasColumnName("ativo_id")
            .HasColumnType("BIGINT UNSIGNED")
            .IsRequired();

        builder.HasOne(t => t.Asset)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AssetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.PositionSize)
            .HasColumnName("quantidade")
            .HasColumnType("integer")
            .IsRequired();

        builder.Property(t => t.UnitPrice)
            .HasColumnName("preco_unitario")
            .HasColumnType("decimal(10, 2)")
            .IsRequired();

        builder.Property(t => t.Type)
            .HasColumnName("tipo_operacao")
            .HasConversion<int>()
            .HasColumnType("TINYINT")
            .IsRequired();

        builder.Property(t => t.BrokerageValue)
            .HasColumnName("corretagem")
            .HasColumnType("decimal(10, 2)")
            .IsRequired();
    }
}
