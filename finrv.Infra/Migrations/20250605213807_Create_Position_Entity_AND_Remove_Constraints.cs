using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finrv.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Create_Position_Entity_AND_Remove_Constraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "posicao",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "CHAR(36)", nullable: false),
                    usuario_id = table.Column<Guid>(type: "CHAR(36)", nullable: false),
                    ativo_id = table.Column<ulong>(type: "BIGINT UNSIGNED", nullable: false),
                    quantidade = table.Column<uint>(type: "INT UNSIGNED", nullable: false),
                    preco_medio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    lucro_ou_perda = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    criado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: false),
                    criado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: true),
                    atualizado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posicao", x => x.id);
                    table.ForeignKey(
                        name: "FK_posicao_ativo_ativo_id",
                        column: x => x.ativo_id,
                        principalTable: "ativo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_posicao_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_posicao_ativo_id",
                table: "posicao",
                column: "ativo_id");

            migrationBuilder.CreateIndex(
                name: "IX_posicao_usuario_id",
                table: "posicao",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "posicao");
        }
    }
}
