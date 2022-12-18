using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SEP.Authorization.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMicroserviceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMicroserviceType",
                table: "AuthKeys",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMicroserviceType",
                table: "AuthKeys");
        }
    }
}
