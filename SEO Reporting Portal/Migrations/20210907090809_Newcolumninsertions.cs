using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEO_Reporting_Portal.Migrations
{
    public partial class Newcolumninsertions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "6bc63788-0bda-4584-bc2f-b57dc3fc07f0");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "f69adac3-cb6f-462b-b676-8a01ff211b16");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "User",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Company",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a11f831-d003-458c-8268-0e3fc80da4c7", "eb58d25e-9420-4323-9561-e76154a8ed66", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fcc8530c-fce9-4f79-8f3b-a42bfeb6109e", "0a8555e6-1861-4948-a10e-3d18ab9af2a2", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "5a11f831-d003-458c-8268-0e3fc80da4c7");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "fcc8530c-fce9-4f79-8f3b-a42bfeb6109e");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Company");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f69adac3-cb6f-462b-b676-8a01ff211b16", "bbbef937-d92b-43b2-813d-ab4323c72cc0", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6bc63788-0bda-4584-bc2f-b57dc3fc07f0", "32382176-6881-4a43-b3d6-d3f06ec8deaa", "User", "USER" });
        }
    }
}
