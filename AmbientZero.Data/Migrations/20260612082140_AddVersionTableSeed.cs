using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AmbientZero.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVersionTableSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "version",
                columns: new[] { "Id", "LastUpdatedOn", "LocalVersion", "ModuleName", "RemoteVersion" },
                values: new object[,]
                {
                    { 1, null, "0.0.0", "API", null },
                    { 2, null, "0.0.0", "PCB", null },
                    { 3, null, "0.0.0", "Web", null },
                    { 4, null, "0.0.0", "Sensor Code", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "version",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "version",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "version",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "version",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
