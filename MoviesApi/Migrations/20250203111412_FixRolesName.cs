using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoviesApi.Migrations
{
    /// <inheritdoc />
    public partial class FixRolesName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af182f45-f70a-451b-9d10-00acb3560436");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c89412a2-1949-4afb-94b2-feca3f5a7366");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "88fb9792-445b-4272-ab6c-002c2c8c7d1c", null, "admin", "ADMIN" },
                    { "ddf956bb-d75d-4c99-a85a-f963e64f93b9", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88fb9792-445b-4272-ab6c-002c2c8c7d1c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddf956bb-d75d-4c99-a85a-f963e64f93b9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "af182f45-f70a-451b-9d10-00acb3560436", null, "User", "ADMIN" },
                    { "c89412a2-1949-4afb-94b2-feca3f5a7366", null, "User", "USER" }
                });
        }
    }
}
