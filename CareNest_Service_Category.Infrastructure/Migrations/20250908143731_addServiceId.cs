using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareNest_Service_Category.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addServiceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Service_Id",
                table: "ServiceCategories",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Service_Id",
                table: "ServiceCategories");
        }
    }
}
