using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEP.PayPal.Migrations
{
    /// <inheritdoc />
    public partial class IdentityToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MerchantID",
                table: "PayPalPayment",
                newName: "MerchantId");

            migrationBuilder.AddColumn<string>(
                name: "IdentityToken",
                table: "PayPalPayment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityToken",
                table: "PayPalPayment");

            migrationBuilder.RenameColumn(
                name: "MerchantId",
                table: "PayPalPayment",
                newName: "MerchantID");
        }
    }
}
