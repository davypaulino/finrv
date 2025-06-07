using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finrv.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Added_index_for_search_asset_by_ticker_break : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_latest_quotation_by_ticker_des",
                table: "cotacao");

            migrationBuilder.CreateIndex(
                name: "IX_latest_quotation_by_ticker_createdAt_des",
                table: "cotacao",
                columns: new[] { "ativo_id", "criado_em" },
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_latest_quotation_by_ticker_updateAt_des",
                table: "cotacao",
                columns: new[] { "ativo_id", "atualizado_em" },
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_latest_quotation_by_ticker_createdAt_des",
                table: "cotacao");

            migrationBuilder.DropIndex(
                name: "IX_latest_quotation_by_ticker_updateAt_des",
                table: "cotacao");

            migrationBuilder.CreateIndex(
                name: "IX_latest_quotation_by_ticker_des",
                table: "cotacao",
                columns: new[] { "ativo_id", "criado_em", "atualizado_em" },
                descending: new bool[0]);
        }
    }
}
