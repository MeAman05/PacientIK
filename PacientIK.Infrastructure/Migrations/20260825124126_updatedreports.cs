using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PacientIK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedreports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Leches_LechId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropIndex(
                name: "IX_Reports_LechId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "LechId",
                table: "Reports");

            migrationBuilder.CreateTable(
                name: "LechReport",
                columns: table => new
                {
                    LechesId = table.Column<int>(type: "integer", nullable: false),
                    ReportsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LechReport", x => new { x.LechesId, x.ReportsId });
                    table.ForeignKey(
                        name: "FK_LechReport_Leches_LechesId",
                        column: x => x.LechesId,
                        principalTable: "Leches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LechReport_Reports_ReportsId",
                        column: x => x.ReportsId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LechReport_ReportsId",
                table: "LechReport",
                column: "ReportsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LechReport");

            migrationBuilder.AddColumn<int>(
                name: "LechId",
                table: "Reports",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    TEstName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LechId",
                table: "Reports",
                column: "LechId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Leches_LechId",
                table: "Reports",
                column: "LechId",
                principalTable: "Leches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
