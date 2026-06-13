using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AmbientZero.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "applicationSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SettingName = table.Column<string>(type: "TEXT", nullable: false),
                    SettingValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicationSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sensorSetting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: false),
                    IsLightEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsLightOnSchedule = table.Column<bool>(type: "INTEGER", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    DisableStartTime = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    DisableEndTime = table.Column<TimeOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensorSetting", x => x.Id);
                    table.UniqueConstraint("AK_sensorSetting_DeviceId", x => x.DeviceId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "deviceInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: false),
                    DeviceVirtualName = table.Column<string>(type: "TEXT", nullable: true),
                    ADCReading = table.Column<int>(type: "INTEGER", nullable: true),
                    IsSignaling = table.Column<bool>(type: "INTEGER", nullable: true),
                    Version = table.Column<string>(type: "TEXT", nullable: true),
                    IsMigrated = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deviceInformation", x => x.Id);
                    table.UniqueConstraint("AK_deviceInformation_DeviceId", x => x.DeviceId);
                    table.ForeignKey(
                        name: "FK_deviceInformation_sensorSetting_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "sensorSetting",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "measurementLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceId = table.Column<string>(type: "TEXT", nullable: false),
                    Temperature = table.Column<float>(type: "REAL", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_measurementLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_measurementLog_deviceInformation_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "deviceInformation",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "applicationSetting",
                columns: new[] { "Id", "SettingName", "SettingValue" },
                values: new object[,]
                {
                    { 1, "TimeZone", "UTC" },
                    { 2, "TemperatureSetting", "1" },
                    { 3, "ApplicationSecurity", "1" },
                    { 4, "IsFirstRun", "1" },
                    { 5, "AccessPointName", "AmbientZero" },
                    { 6, "AccessPointSecurity", "0" },
                    { 7, "AccessPointPassword", null },
                    { 8, "CheckForUpdatesEnabled", "1" },
                    { 9, "AutomaticUpdatesEnabled", "0" },
                    { 10, "MaxBatteryAdcReading", "36024" },
                    { 11, "MaxBatteryAdcReading", "14915" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedOn", "IsActive", "Password", "UpdatedOn", "Username" },
                values: new object[] { 1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "", null, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_measurementLog_DeviceId",
                table: "measurementLog",
                column: "DeviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "applicationSetting");

            migrationBuilder.DropTable(
                name: "measurementLog");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "deviceInformation");

            migrationBuilder.DropTable(
                name: "sensorSetting");
        }
    }
}
