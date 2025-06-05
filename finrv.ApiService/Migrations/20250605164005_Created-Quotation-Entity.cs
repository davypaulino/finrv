using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finrv.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class CreatedQuotationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cotacao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "CHAR(36)", nullable: false),
                    ativo_id = table.Column<ulong>(type: "BIGINT UNSIGNED", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    criado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: false),
                    criado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: true),
                    atualizado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cotacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_cotacao_ativo_ativo_id",
                        column: x => x.ativo_id,
                        principalTable: "ativo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_cotacao_ativo_id",
                table: "cotacao",
                column: "ativo_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cotacao");
        }
    }
}
