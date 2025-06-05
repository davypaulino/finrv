using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace finrv.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class CreatedAssetEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ativo",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "serial", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    codigo = table.Column<string>(type: "varchar(10)", nullable: false),
                    nome = table.Column<string>(type: "varchar(150)", nullable: false),
                    criado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: false),
                    criado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: true),
                    atualizado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ativo", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ativo");
        }
    }
}
