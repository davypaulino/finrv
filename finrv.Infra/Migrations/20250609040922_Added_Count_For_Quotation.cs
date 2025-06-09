using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace finrv.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Added_Count_For_Quotation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<ulong>(
                name: "contador",
                table: "cotacao",
                type: "BIGINT UNSIGNED",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contador",
                table: "cotacao");
        }
    }
}
