using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEO_Reporting_Portal.Migrations
{
    public partial class addedGeneralInquiryentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "4beb6855-d3a0-440e-bf44-a5bbd0c9b30f");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "5616a02a-a3fb-4c6c-86f6-f92ca22a4bd9");

            migrationBuilder.CreateTable(
                name: "GeneralInquiries",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    RespondentId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralInquiries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralInquiries_User_RespondentId",
                        column: x => x.RespondentId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralInquiries_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b67bbc54-cb6e-4ed9-b8c5-5bc4c4582614", "ae3c6b9c-6d70-4e9c-ad5a-bd7b738c84eb", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d2448102-e91f-4ffe-9af6-6f4434389c05", "372297c9-41f7-42d9-8e51-bf9ce8e12e96", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInquiries_RespondentId",
                table: "GeneralInquiries",
                column: "RespondentId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralInquiries_UserId",
                table: "GeneralInquiries",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralInquiries");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "b67bbc54-cb6e-4ed9-b8c5-5bc4c4582614");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "d2448102-e91f-4ffe-9af6-6f4434389c05");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4beb6855-d3a0-440e-bf44-a5bbd0c9b30f", "ffd0c934-65d7-45f9-a2a5-6938e5d535f2", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5616a02a-a3fb-4c6c-86f6-f92ca22a4bd9", "fe3be25a-3f17-40ee-b8f2-b5e67a0296e3", "User", "USER" });
        }
    }
}
