using finrv.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace finrv.Domain.Mapping;

public class UserEntityMapping : BaseEntityMapping<UserEntity>
{
    protected override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("usuario");

        builder.Property(u => u.Name)
            .HasColumnName("nome")
            .HasColumnType("varchar(100)")
            .IsRequired();

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .HasColumnType("varchar(150)")
            .IsRequired();

        builder.Property(u => u.BrokeragePercent)
            .HasColumnName("porcentagem_corretagem")
            .HasColumnType("decimal(10, 2)")
            .IsRequired();
    }
}
