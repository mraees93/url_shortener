using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace url_shortener.Migrations
{
    /// <inheritdoc />
    public partial class ReadyToLaunch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ShortUrls",
                table: "ShortUrls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClickEvents",
                table: "ClickEvents");

            migrationBuilder.RenameTable(
                name: "ShortUrls",
                newName: "FinalLinks");

            migrationBuilder.RenameTable(
                name: "ClickEvents",
                newName: "FinalClicks");

            migrationBuilder.RenameIndex(
                name: "IX_ShortUrls_ShortCode",
                table: "FinalLinks",
                newName: "IX_FinalLinks_ShortCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalLinks",
                table: "FinalLinks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinalClicks",
                table: "FinalClicks",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalLinks",
                table: "FinalLinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinalClicks",
                table: "FinalClicks");

            migrationBuilder.RenameTable(
                name: "FinalLinks",
                newName: "ShortUrls");

            migrationBuilder.RenameTable(
                name: "FinalClicks",
                newName: "ClickEvents");

            migrationBuilder.RenameIndex(
                name: "IX_FinalLinks_ShortCode",
                table: "ShortUrls",
                newName: "IX_ShortUrls_ShortCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShortUrls",
                table: "ShortUrls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClickEvents",
                table: "ClickEvents",
                column: "Id");
        }
    }
}
