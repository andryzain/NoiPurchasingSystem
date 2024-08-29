using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiPurchasingSystem.Migrations
{
    public partial class initializeBerat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BeratId",
                schema: "dbo",
                table: "MstrProduk",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MstrBerat",
                schema: "dbo",
                columns: table => new
                {
                    BeratId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeBerat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nilai = table.Column<int>(type: "int", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeleteDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsCancel = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstrBerat", x => x.BeratId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstrProduk_BeratId",
                schema: "dbo",
                table: "MstrProduk",
                column: "BeratId");

            migrationBuilder.AddForeignKey(
                name: "FK_MstrProduk_MstrBerat_BeratId",
                schema: "dbo",
                table: "MstrProduk",
                column: "BeratId",
                principalSchema: "dbo",
                principalTable: "MstrBerat",
                principalColumn: "BeratId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MstrProduk_MstrBerat_BeratId",
                schema: "dbo",
                table: "MstrProduk");

            migrationBuilder.DropTable(
                name: "MstrBerat",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_MstrProduk_BeratId",
                schema: "dbo",
                table: "MstrProduk");

            migrationBuilder.DropColumn(
                name: "BeratId",
                schema: "dbo",
                table: "MstrProduk");
        }
    }
}
