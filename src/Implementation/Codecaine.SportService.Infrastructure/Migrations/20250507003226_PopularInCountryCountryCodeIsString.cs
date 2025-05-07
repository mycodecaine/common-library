using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Codecaine.SportService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PopularInCountryCountryCodeIsString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "PopularInCountry",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CountryCode",
                table: "PopularInCountry",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
