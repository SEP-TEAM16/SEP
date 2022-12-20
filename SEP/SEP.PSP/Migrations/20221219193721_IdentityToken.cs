using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEP.PSP.Migrations
{
    /// <inheritdoc />
    public partial class IdentityToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityToken",
                table: "PSPPayments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityToken",
                table: "PSPPayments");
        }
    }
}
