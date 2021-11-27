using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.Migrations
{
    public partial class addedAPptypeInProd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Products",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ApplicationId",
                table: "Products",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationId",
                table: "Products",
                column: "ApplicationId",
                principalTable: "ApplicationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ApplicationTypes_ApplicationId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ApplicationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Products");
        }
    }
}
