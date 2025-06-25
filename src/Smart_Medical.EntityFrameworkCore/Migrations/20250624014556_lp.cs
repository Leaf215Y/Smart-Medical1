using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Smart_Medical.Migrations
{
    /// <inheritdoc />
    public partial class lp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "AppPharmaceuticalCompanies");

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "AppPharmaceuticalCompanies",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "AppPharmaceuticalCompanies",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AppPharmaceuticalCompanies",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "AppPharmaceuticalCompanies");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "AppPharmaceuticalCompanies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AppPharmaceuticalCompanies");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "AppPharmaceuticalCompanies",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");
        }
    }
}
