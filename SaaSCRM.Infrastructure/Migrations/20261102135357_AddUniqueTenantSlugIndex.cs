using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaSCRM.Infrastructure.Migrations
{
    public partial class AddUniqueTenantSlugIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tenants_Slug",
                table: "Tenants",
                column: "Slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tenants_Slug",
                table: "Tenants");
        }
    }
}