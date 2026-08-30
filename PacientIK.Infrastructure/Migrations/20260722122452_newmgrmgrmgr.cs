using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PacientIK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newmgrmgrmgr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhotoName",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Spec",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spec", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_SpecId",
                table: "Users",
                column: "SpecId");

            migrationBuilder.CreateIndex(
                name: "IX_Leches_SpecId",
                table: "Leches",
                column: "SpecId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leches_Spec_SpecId",
                table: "Leches",
                column: "SpecId",
                principalTable: "Spec",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Spec_SpecId",
                table: "Users",
                column: "SpecId",
                principalTable: "Spec",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leches_Spec_SpecId",
                table: "Leches");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Spec_SpecId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Spec");

            migrationBuilder.DropIndex(
                name: "IX_Users_SpecId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Leches_SpecId",
                table: "Leches");

            migrationBuilder.AlterColumn<string>(
                name: "PhotoName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
