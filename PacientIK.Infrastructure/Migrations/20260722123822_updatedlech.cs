using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PacientIK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedlech : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Leches_LechId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_LechId",
                table: "Reports");
        }
    }
}
