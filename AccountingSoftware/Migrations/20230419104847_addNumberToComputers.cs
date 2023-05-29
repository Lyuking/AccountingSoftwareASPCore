using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSoftware.Migrations
{
    public partial class addNumberToComputers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Computers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Computers");
        }
    }
}
