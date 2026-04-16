using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GravitySwingData.Migrations
{
    /// <inheritdoc />
    public partial class removeredundantuserattributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRecord_Users_UserId",
                table: "GameRecord");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameRecord",
                table: "GameRecord");

            migrationBuilder.DropColumn(
                name: "LongestCombo",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "GameRecord",
                newName: "GameRecords");

            migrationBuilder.RenameIndex(
                name: "IX_GameRecord_UserId",
                table: "GameRecords",
                newName: "IX_GameRecords_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameRecords",
                table: "GameRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRecords_Users_UserId",
                table: "GameRecords",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRecords_Users_UserId",
                table: "GameRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameRecords",
                table: "GameRecords");

            migrationBuilder.RenameTable(
                name: "GameRecords",
                newName: "GameRecord");

            migrationBuilder.RenameIndex(
                name: "IX_GameRecords_UserId",
                table: "GameRecord",
                newName: "IX_GameRecord_UserId");

            migrationBuilder.AddColumn<int>(
                name: "LongestCombo",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameRecord",
                table: "GameRecord",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRecord_Users_UserId",
                table: "GameRecord",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
