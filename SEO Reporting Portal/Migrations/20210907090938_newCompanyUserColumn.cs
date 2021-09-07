using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SEO_Reporting_Portal.Migrations
{
    public partial class newCompanyUserColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "5a11f831-d003-458c-8268-0e3fc80da4c7");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "fcc8530c-fce9-4f79-8f3b-a42bfeb6109e");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CompanyUsers",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5768273a-83b6-4723-8319-a7d1134f4bdf", "e3a2d0bb-36f6-4be3-8f46-6bd67cd464b7", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "db4a5f32-62f3-42a3-9e57-ed7172980927", "016e4009-fa9a-49ba-9e98-53c4f47310c1", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "5768273a-83b6-4723-8319-a7d1134f4bdf");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: "db4a5f32-62f3-42a3-9e57-ed7172980927");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CompanyUsers");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5a11f831-d003-458c-8268-0e3fc80da4c7", "eb58d25e-9420-4323-9561-e76154a8ed66", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fcc8530c-fce9-4f79-8f3b-a42bfeb6109e", "0a8555e6-1861-4948-a10e-3d18ab9af2a2", "User", "USER" });
        }
    }
}
