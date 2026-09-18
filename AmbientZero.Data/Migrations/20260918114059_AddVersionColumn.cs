using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmbientZero.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVersionColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "sensorSetting",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "sensorSetting");
        }
    }
}
