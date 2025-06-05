using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace finrv.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class Refactory_Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ativo",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "BIGINT UNSIGNED", nullable: false)
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
                    table.PrimaryKey("PK_ativo", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "CHAR(36)", nullable: false),
                    nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    email = table.Column<string>(type: "varchar(150)", nullable: false),
                    porcentagem_corretagem = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    criado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: false),
                    criado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: true),
                    atualizado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cotacao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "BIGINT UNSIGNED", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operacoes",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "BIGINT UNSIGNED", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<Guid>(type: "CHAR(36)", nullable: false),
                    ativo_id = table.Column<ulong>(type: "BIGINT UNSIGNED", nullable: false),
                    quantidade = table.Column<int>(type: "integer", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    tipo_operacao = table.Column<sbyte>(type: "TINYINT", nullable: false),
                    corretagem = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    criado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: false),
                    criado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    atualizado_em = table.Column<DateTime>(type: "DATETIME(6)", nullable: true),
                    atualizado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operacoes", x => x.id);
                    table.ForeignKey(
                        name: "FK_operacoes_ativo_ativo_id",
                        column: x => x.ativo_id,
                        principalTable: "ativo",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_operacoes_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_cotacao_ativo_id",
                table: "cotacao",
                column: "ativo_id");

            migrationBuilder.CreateIndex(
                name: "IX_operacoes_ativo_id",
                table: "operacoes",
                column: "ativo_id");

            migrationBuilder.CreateIndex(
                name: "IX_operacoes_usuario_id",
                table: "operacoes",
                column: "usuario_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cotacao");

            migrationBuilder.DropTable(
                name: "operacoes");

            migrationBuilder.DropTable(
                name: "ativo");

            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
