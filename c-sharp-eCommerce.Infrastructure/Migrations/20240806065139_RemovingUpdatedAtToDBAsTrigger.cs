using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace c_sharp_eCommerce.Infrastructure.Migrations
{
    public partial class RemovingUpdatedAtToDBAsTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Products",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "GETUTCDATE()");
        }
    }
}
