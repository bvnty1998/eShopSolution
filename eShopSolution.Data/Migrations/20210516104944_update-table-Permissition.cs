using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class updatetablePermissition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FunctionName",
                table: "Permissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "Permissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FunctionName",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "Permissions");
        }
    }
}
