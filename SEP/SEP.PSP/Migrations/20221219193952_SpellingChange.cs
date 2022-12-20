using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEP.PSP.Migrations
{
    /// <inheritdoc />
    public partial class SpellingChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PSPPayments_Metchants_MerchantId",
                table: "PSPPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Metchants_MerchantId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Metchants",
                table: "Metchants");

            migrationBuilder.RenameTable(
                name: "Metchants",
                newName: "Merchants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PSPPayments_Merchants_MerchantId",
                table: "PSPPayments",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Merchants_MerchantId",
                table: "Subscriptions",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PSPPayments_Merchants_MerchantId",
                table: "PSPPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Merchants_MerchantId",
                table: "Subscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants");

            migrationBuilder.RenameTable(
                name: "Merchants",
                newName: "Metchants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Metchants",
                table: "Metchants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PSPPayments_Metchants_MerchantId",
                table: "PSPPayments",
                column: "MerchantId",
                principalTable: "Metchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Metchants_MerchantId",
                table: "Subscriptions",
                column: "MerchantId",
                principalTable: "Metchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
