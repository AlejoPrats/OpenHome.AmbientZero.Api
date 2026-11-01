using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AmbientZero.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifySeedsCreateCustomColorTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "sensorSetting");

            migrationBuilder.CreateTable(
                name: "chartColor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chartColor", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "applicationSetting",
                columns: new[] { "Id", "SettingName", "SettingValue" },
                values: new object[,]
                {
                    { 20, "LastBackupDate", null },
                    { 21, "DeviceName", "AmbientZero" },
                    { 22, "AccessPointChannel", "6" }
                });

            migrationBuilder.InsertData(
                table: "chartColor",
                columns: new[] { "Id", "Color", "IsUsed", "Priority" },
                values: new object[,]
                {
                    { 1, "#5070dd", false, 1 },
                    { 2, "#b6d634", false, 1 },
                    { 3, "#505372", false, 1 },
                    { 4, "#ff994d", false, 1 },
                    { 5, "#0ca8df", false, 1 },
                    { 6, "#ffd10a", false, 1 },
                    { 7, "#fb628b", false, 1 },
                    { 8, "#785db0", false, 1 },
                    { 9, "#3fbe95", false, 1 },
                    { 10, "#1d3997", false, 2 },
                    { 11, "#6f841a", false, 2 },
                    { 12, "#303144", false, 2 },
                    { 13, "#c75500", false, 2 },
                    { 14, "#076485", false, 2 },
                    { 15, "#9f8100", false, 2 },
                    { 16, "#cc053a", false, 2 },
                    { 17, "#46346c", false, 2 },
                    { 18, "#257259", false, 2 },
                    { 19, "#96a9ea", false, 3 },
                    { 20, "#d3e685", false, 3 },
                    { 21, "#8f92b0", false, 3 },
                    { 22, "#fec194", false, 3 },
                    { 23, "#62cff6", false, 3 },
                    { 24, "#ffe36c", false, 3 },
                    { 25, "#fca0b9", false, 3 },
                    { 26, "#ae9dcf", false, 3 },
                    { 27, "#8bd8bf", false, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chartColor");

            migrationBuilder.DeleteData(
                table: "applicationSetting",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "applicationSetting",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "applicationSetting",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "sensorSetting",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
