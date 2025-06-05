using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finrv.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class Initialdatabasecreateuserentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "usuario");
        }
    }
}
