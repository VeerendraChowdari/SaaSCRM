using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaSCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixTenantSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Tenants",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Tenants");
        }
    }
}
