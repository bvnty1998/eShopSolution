using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class updateIdentityfortableFunction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUserRoles_FunctionId",
                table: "AppUserRoles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Functions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Functions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_FunctionId",
                table: "AppUserRoles",
                column: "FunctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppUserRoles_FunctionId",
                table: "AppUserRoles");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Functions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "Functions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_FunctionId",
                table: "AppUserRoles",
                column: "FunctionId",
                unique: true);
        }
    }
}
