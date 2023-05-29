using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSoftware.Migrations
{
    public partial class ChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QRURL",
                table: "SoftwareTechnicalDetailses",
                newName: "Photo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                table: "SoftwareTechnicalDetailses",
                newName: "QRURL");
        }
    }
}
