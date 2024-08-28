using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NoiPurchasingSystem.Migrations
{
    public partial class initializePrpo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrpoPurchaseRequest",
                schema: "dbo",
                columns: table => new
                {
                    PurchaseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAccessId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserApprovalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetodePembayaranId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QtyTotal = table.Column<int>(type: "int", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermOfPaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_PrpoPurchaseRequest", x => x.PurchaseRequestId);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseRequest_AspNetUsers_UserAccessId",
                        column: x => x.UserAccessId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseRequest_MstrMetodePembayaran_TermOfPaymentId",
                        column: x => x.TermOfPaymentId,
                        principalSchema: "dbo",
                        principalTable: "MstrMetodePembayaran",
                        principalColumn: "MetodePembayaranId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseRequest_MstrPengguna_UserApprovalId",
                        column: x => x.UserApprovalId,
                        principalSchema: "dbo",
                        principalTable: "MstrPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrpoPurchaseOrder",
                schema: "dbo",
                columns: table => new
                {
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseRequestNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAccessId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserApprovalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MetodePembayaranId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QtyTotal = table.Column<int>(type: "int", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_PrpoPurchaseOrder", x => x.PurchaseOrderId);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseOrder_AspNetUsers_UserAccessId",
                        column: x => x.UserAccessId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseOrder_MstrMetodePembayaran_MetodePembayaranId",
                        column: x => x.MetodePembayaranId,
                        principalSchema: "dbo",
                        principalTable: "MstrMetodePembayaran",
                        principalColumn: "MetodePembayaranId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseOrder_MstrPengguna_UserApprovalId",
                        column: x => x.UserApprovalId,
                        principalSchema: "dbo",
                        principalTable: "MstrPengguna",
                        principalColumn: "PenggunaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseOrder_PrpoPurchaseRequest_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "dbo",
                        principalTable: "PrpoPurchaseRequest",
                        principalColumn: "PurchaseRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrpoPurchaseRequestDetail",
                schema: "dbo",
                columns: table => new
                {
                    PurchaseRequestDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Principal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_PrpoPurchaseRequestDetail", x => x.PurchaseRequestDetailId);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseRequestDetail_PrpoPurchaseRequest_PurchaseRequestId",
                        column: x => x.PurchaseRequestId,
                        principalSchema: "dbo",
                        principalTable: "PrpoPurchaseRequest",
                        principalColumn: "PurchaseRequestId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PrpoPurchaseOrderDetail",
                schema: "dbo",
                columns: table => new
                {
                    PurchaseOrderDetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Measurement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Principal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_PrpoPurchaseOrderDetail", x => x.PurchaseOrderDetailId);
                    table.ForeignKey(
                        name: "FK_PrpoPurchaseOrderDetail_PrpoPurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "dbo",
                        principalTable: "PrpoPurchaseOrder",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseOrder_MetodePembayaranId",
                schema: "dbo",
                table: "PrpoPurchaseOrder",
                column: "MetodePembayaranId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseOrder_PurchaseRequestId",
                schema: "dbo",
                table: "PrpoPurchaseOrder",
                column: "PurchaseRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseOrder_UserAccessId",
                schema: "dbo",
                table: "PrpoPurchaseOrder",
                column: "UserAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseOrder_UserApprovalId",
                schema: "dbo",
                table: "PrpoPurchaseOrder",
                column: "UserApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseOrderDetail_PurchaseOrderId",
                schema: "dbo",
                table: "PrpoPurchaseOrderDetail",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseRequest_TermOfPaymentId",
                schema: "dbo",
                table: "PrpoPurchaseRequest",
                column: "TermOfPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseRequest_UserAccessId",
                schema: "dbo",
                table: "PrpoPurchaseRequest",
                column: "UserAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseRequest_UserApprovalId",
                schema: "dbo",
                table: "PrpoPurchaseRequest",
                column: "UserApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_PrpoPurchaseRequestDetail_PurchaseRequestId",
                schema: "dbo",
                table: "PrpoPurchaseRequestDetail",
                column: "PurchaseRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrpoPurchaseOrderDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PrpoPurchaseRequestDetail",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PrpoPurchaseOrder",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PrpoPurchaseRequest",
                schema: "dbo");
        }
    }
}
