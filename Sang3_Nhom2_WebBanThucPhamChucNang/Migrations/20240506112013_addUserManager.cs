using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sang3_Nhom2_WebBanThucPhamChucNang.Migrations
{
    /// <inheritdoc />
    public partial class addUserManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isLooked",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isLooked",
                table: "AspNetUsers");
        }
    }
}
