using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaaSCRM.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingRegistrationCompletedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedOn",
                table: "PendingRegistrations",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedOn",
                table: "PendingRegistrations");
        }
    }
}
