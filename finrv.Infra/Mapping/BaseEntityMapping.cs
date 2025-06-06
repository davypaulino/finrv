using finrv.Domain.Entities;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace finrv.Infra.Mapping;

public abstract class BaseEntityMapping<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    void IEntityTypeConfiguration<T>.Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(b => b.CreatedBy)
            .HasColumnName("criado_por")
            .HasMaxLength(100);

        builder.Property(b => b.CreatedAt)
            .HasColumnName("criado_em")
            .HasColumnType("DATETIME(6)");

        builder.Property(b => b.UpdatedBy)
            .HasColumnName("atualizado_por")
            .HasMaxLength(100);

        builder.Property(b => b.UpdatedAt)
            .HasColumnName("atualizado_em")
            .HasColumnType("DATETIME(6)")
            .ValueGeneratedOnUpdate();

        Configure(builder);

    }

    protected abstract void Configure(EntityTypeBuilder<T> builder);
}