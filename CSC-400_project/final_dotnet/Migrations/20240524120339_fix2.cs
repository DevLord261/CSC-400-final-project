using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace final_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_campaigns_Users_Id",
                table: "campaigns");

            migrationBuilder.CreateIndex(
                name: "IX_campaigns_ownerId",
                table: "campaigns",
                column: "ownerId");

            migrationBuilder.AddForeignKey(
                name: "FK_campaigns_Users_ownerId",
                table: "campaigns",
                column: "ownerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_campaigns_Users_ownerId",
                table: "campaigns");

            migrationBuilder.DropIndex(
                name: "IX_campaigns_ownerId",
                table: "campaigns");

            migrationBuilder.AddForeignKey(
                name: "FK_campaigns_Users_Id",
                table: "campaigns",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
