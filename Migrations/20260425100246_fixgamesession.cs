using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GravitySwingData.Migrations
{
    /// <inheritdoc />
    public partial class fixgamesession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameRecords_GameRecordId",
                table: "GameSessions");

            migrationBuilder.AlterColumn<int>(
                name: "GameRecordId",
                table: "GameSessions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameRecords_GameRecordId",
                table: "GameSessions",
                column: "GameRecordId",
                principalTable: "GameRecords",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameSessions_GameRecords_GameRecordId",
                table: "GameSessions");

            migrationBuilder.AlterColumn<int>(
                name: "GameRecordId",
                table: "GameSessions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GameSessions_GameRecords_GameRecordId",
                table: "GameSessions",
                column: "GameRecordId",
                principalTable: "GameRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
