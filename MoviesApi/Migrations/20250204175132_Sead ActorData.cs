using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoviesApi.Migrations
{
    /// <inheritdoc />
    public partial class SeadActorData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "baec89d5-b293-4d85-b020-53fa9f526ac1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9af430b-34fb-413c-bd13-281952ee956c");

            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "Biography", "DateOfBirth", "Name", "Picture" },
                values: new object[,]
                {
                    { 1, "An American actor and film producer, known for his diverse roles and dedication to his craft.", new DateOnly(1974, 11, 11), "Leonardo DiCaprio", "" },
                    { 2, "An English actress known for her roles in dramatic films.", new DateOnly(1975, 10, 5), "Kate Winslet", "" },
                    { 3, "An American actor and film producer, noted for his charismatic screen presence.", new DateOnly(1963, 12, 18), "Brad Pitt", "" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "55288671-df33-47c1-817c-91f546c7adba", null, "admin", "ADMIN" },
                    { "79415083-c390-4f35-b519-39363e99fab3", null, "user", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "55288671-df33-47c1-817c-91f546c7adba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79415083-c390-4f35-b519-39363e99fab3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "baec89d5-b293-4d85-b020-53fa9f526ac1", null, "user", "USER" },
                    { "d9af430b-34fb-413c-bd13-281952ee956c", null, "admin", "ADMIN" }
                });
        }
    }
}
