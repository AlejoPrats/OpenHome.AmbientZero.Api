using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    SettingValue = table.Column<string>(type: "TEXT", nullable: false)
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
                    DisableStartTime = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    DisableEndTime = table.Column<TimeOnly>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensorSetting", x => x.Id);
                    table.UniqueConstraint("AK_sensorSetting_DeviceId", x => x.DeviceId);
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
                    Version = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "deviceInformation");

            migrationBuilder.DropTable(
                name: "sensorSetting");
        }
    }
}
