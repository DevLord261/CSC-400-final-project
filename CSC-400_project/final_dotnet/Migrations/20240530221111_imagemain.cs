using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace final_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class imagemain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mainImage",
                table: "campaigns",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mainImage",
                table: "campaigns");
        }
    }
}
