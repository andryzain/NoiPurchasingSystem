using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiPurchasingSystem.Migrations
{
    public partial class addBeratInProductPRPO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Weight",
                schema: "dbo",
                table: "PrpoPurchaseRequestDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                schema: "dbo",
                table: "PrpoPurchaseOrderDetail",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "dbo",
                table: "PrpoPurchaseRequestDetail");

            migrationBuilder.DropColumn(
                name: "Weight",
                schema: "dbo",
                table: "PrpoPurchaseOrderDetail");
        }
    }
}
