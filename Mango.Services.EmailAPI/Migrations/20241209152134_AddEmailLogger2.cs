using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.EmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailLogger2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "EmailLoggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailLoggers",
                table: "EmailLoggers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailLoggers",
                table: "EmailLoggers");

            migrationBuilder.RenameTable(
                name: "EmailLoggers",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");
        }
    }
}
