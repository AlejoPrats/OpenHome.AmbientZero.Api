using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmbientZero.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVersionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "version",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleName = table.Column<string>(type: "TEXT", nullable: false),
                    LocalVersion = table.Column<string>(type: "TEXT", nullable: false),
                    RemoteVersion = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_version", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "version");
        }
    }
}
