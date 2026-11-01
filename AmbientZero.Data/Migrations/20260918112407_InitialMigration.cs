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
                name: "migrationReferenceId",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OriginalId = table.Column<string>(type: "TEXT", nullable: false),
                    NewId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_migrationReferenceId", x => x.Id);
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
                    DisableStartTime = table.Column<string>(type: "TEXT", nullable: true),
                    DisableEndTime = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "version",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleId = table.Column<string>(type: "TEXT", nullable: false),
                    ModuleName = table.Column<string>(type: "TEXT", nullable: false),
                    LocalVersion = table.Column<string>(type: "TEXT", nullable: false),
                    RemoteVersion = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_version", x => x.Id);
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
                    { 11, "MinBatteryAdcReading", "14915" },
                    { 12, "RootProjectURL", "https://openhomeproject.com" },
                    { 13, "UpdateEndpoint", "api/Version/GetVersionsByProductId?id=" },
                    { 14, "RemoteDeviceId", "419a522b-8c31-4b44-b9a8-980cb29f1492" },
                    { 15, "IsFirstRunSetup", "1" },
                    { 16, "DefaultLanguage", "en" },
                    { 17, "DefaultUnit", "1" },
                    { 18, "BootTime", "0" },
                    { 19, "SshPassword", "devpass" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "CreatedOn", "IsActive", "Password", "UpdatedOn", "Username" },
                values: new object[] { 1, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "", null, "Admin" });

            migrationBuilder.InsertData(
                table: "version",
                columns: new[] { "Id", "LastUpdatedOn", "LocalVersion", "ModuleId", "ModuleName", "RemoteVersion" },
                values: new object[,]
                {
                    { 1, null, "0.0.0", "19f09be7-36bf-47f9-82ad-19b05a95844e", "API", null },
                    { 2, null, "0.0.0", "4f3f433b-4716-4006-9e38-5c900d8c0849", "PCB", null },
                    { 3, null, "0.0.0", "880c421d-75c7-4015-aec7-e9582cf5f64a", "Web", null },
                    { 4, null, "0.0.0", "c7a09fee-6b02-4c05-9b30-634869e73d65", "Sensor Code", null }
                });

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
                name: "migrationReferenceId");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "version");

            migrationBuilder.DropTable(
                name: "deviceInformation");

            migrationBuilder.DropTable(
                name: "sensorSetting");
        }
    }
}
