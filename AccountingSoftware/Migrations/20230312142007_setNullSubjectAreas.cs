using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSoftware.Migrations
{
    public partial class setNullSubjectAreas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoftwareTechnicalDetailses_SubjectAreas_SubjectAreaId",
                table: "SoftwareTechnicalDetailses");

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwareTechnicalDetailses_SubjectAreas_SubjectAreaId",
                table: "SoftwareTechnicalDetailses",
                column: "SubjectAreaId",
                principalTable: "SubjectAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoftwareTechnicalDetailses_SubjectAreas_SubjectAreaId",
                table: "SoftwareTechnicalDetailses");

            migrationBuilder.AddForeignKey(
                name: "FK_SoftwareTechnicalDetailses_SubjectAreas_SubjectAreaId",
                table: "SoftwareTechnicalDetailses",
                column: "SubjectAreaId",
                principalTable: "SubjectAreas",
                principalColumn: "Id");
        }
    }
}
