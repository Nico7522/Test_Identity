using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_Identity.Migrations
{
    /// <inheritdoc />
    public partial class rajoutstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserStatus",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserStatus",
                table: "AspNetUsers");
        }
    }
}
