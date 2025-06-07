using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finrv.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Added_index_for_search_asset_by_ticker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                table: "ativo",
                type: "varchar(8)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)");

            migrationBuilder.CreateIndex(
                name: "IX_latest_quotation_by_ticker_des",
                table: "cotacao",
                columns: new[] { "ativo_id", "criado_em", "atualizado_em" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Asset_Ticker",
                table: "ativo",
                column: "codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_latest_quotation_by_ticker_des",
                table: "cotacao");

            migrationBuilder.DropIndex(
                name: "IX_Asset_Ticker",
                table: "ativo");

            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                table: "ativo",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(8)");

            migrationBuilder.CreateIndex(
                name: "IX_cotacao_ativo_id",
                table: "cotacao",
                column: "ativo_id");
        }
    }
}
