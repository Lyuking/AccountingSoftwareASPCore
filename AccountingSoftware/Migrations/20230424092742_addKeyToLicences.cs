using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSoftware.Migrations
{
    public partial class addKeyToLicences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "LicenceDetailses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "LicenceDetailses");
        }
    }
}
