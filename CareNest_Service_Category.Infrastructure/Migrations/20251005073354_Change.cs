using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNest_Service_Category.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Service_Id",
                table: "ServiceCategories",
                newName: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "ServiceCategories",
                newName: "Service_Id");
        }
    }
}
