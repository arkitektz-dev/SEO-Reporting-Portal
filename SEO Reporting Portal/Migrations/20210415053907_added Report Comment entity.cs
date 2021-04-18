using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEO_Reporting_Portal.Migrations
{
    public partial class addedReportCommententity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "b67bbc54-cb6e-4ed9-b8c5-5bc4c4582614");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "d2448102-e91f-4ffe-9af6-6f4434389c05");

            migrationBuilder.CreateTable(
                name: "ReportComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Comment = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    ReportId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    RespondentId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportComments_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportComments_User_RespondentId",
                        column: x => x.RespondentId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportComments_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0861e3fd-1c38-4c0c-ae26-b40e7e1d790b", "8f5da908-f732-4852-82d1-0c1a08e6b4b4", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b5b534b9-afa8-4353-9c9f-b7658b187cf0", "95460cc9-9e65-4a81-809f-d7343151616e", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_ReportId",
                table: "ReportComments",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_RespondentId",
                table: "ReportComments",
                column: "RespondentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportComments_UserId",
                table: "ReportComments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportComments");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "0861e3fd-1c38-4c0c-ae26-b40e7e1d790b");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "b5b534b9-afa8-4353-9c9f-b7658b187cf0");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b67bbc54-cb6e-4ed9-b8c5-5bc4c4582614", "ae3c6b9c-6d70-4e9c-ad5a-bd7b738c84eb", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d2448102-e91f-4ffe-9af6-6f4434389c05", "372297c9-41f7-42d9-8e51-bf9ce8e12e96", "User", "USER" });
        }
    }
}
