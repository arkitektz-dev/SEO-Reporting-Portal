using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEO_Reporting_Portal.Migrations
{
    public partial class addedreportentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "61f520fb-9534-461f-a162-5b37450409b6");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "8dc8568f-f44e-46ec-90bf-7881a7eca865");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false),
                    NameExcludingExtenstion = table.Column<string>(type: "varchar(50) CHARACTER SET utf8mb4", maxLength: 50, nullable: false),
                    UniqueName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Path = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    Format = table.Column<string>(type: "varchar(8) CHARACTER SET utf8mb4", maxLength: 8, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    ModifiedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    DeletedBy = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4beb6855-d3a0-440e-bf44-a5bbd0c9b30f", "ffd0c934-65d7-45f9-a2a5-6938e5d535f2", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5616a02a-a3fb-4c6c-86f6-f92ca22a4bd9", "fe3be25a-3f17-40ee-b8f2-b5e67a0296e3", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "4beb6855-d3a0-440e-bf44-a5bbd0c9b30f");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "5616a02a-a3fb-4c6c-86f6-f92ca22a4bd9");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8dc8568f-f44e-46ec-90bf-7881a7eca865", "d10d7575-b048-4afd-895c-eba7451a83de", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "61f520fb-9534-461f-a162-5b37450409b6", "7bc01187-28c1-4daa-a191-060972214bc6", "User", "USER" });
        }
    }
}
