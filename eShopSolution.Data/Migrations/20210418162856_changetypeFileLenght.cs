using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class changetypeFileLenght : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fileSize",
                table: "ProductImages",
                newName: "FileSize");

            migrationBuilder.AlterColumn<long>(
                name: "FileSize",
                table: "ProductImages",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "ProductImages",
                newName: "fileSize");

            migrationBuilder.AlterColumn<int>(
                name: "fileSize",
                table: "ProductImages",
                type: "int",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
