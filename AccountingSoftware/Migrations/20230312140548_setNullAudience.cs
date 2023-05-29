using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSoftware.Migrations
{
    public partial class setNullAudience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computers_Audiences_AudienceId",
                table: "Computers");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_Audiences_AudienceId",
                table: "Computers",
                column: "AudienceId",
                principalTable: "Audiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computers_Audiences_AudienceId",
                table: "Computers");

            migrationBuilder.AddForeignKey(
                name: "FK_Computers_Audiences_AudienceId",
                table: "Computers",
                column: "AudienceId",
                principalTable: "Audiences",
                principalColumn: "Id");
        }
    }
}
