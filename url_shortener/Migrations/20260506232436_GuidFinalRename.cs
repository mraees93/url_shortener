using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace url_shortener.Migrations
{
    /// <inheritdoc />
    public partial class GuidFinalRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clicks_v5_Final",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ShortCode = table.Column<string>(type: "TEXT", nullable: false),
                    ClickedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clicks_v5_Final", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Links_v5_Final",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    LongUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ShortCode = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links_v5_Final", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Links_v5_Final_ShortCode",
                table: "Links_v5_Final",
                column: "ShortCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clicks_v5_Final");

            migrationBuilder.DropTable(
                name: "Links_v5_Final");
        }
    }
}
