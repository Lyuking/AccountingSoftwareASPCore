using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountingSoftware.Migrations
{
    public partial class initialstate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenceDetailses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceDetailses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LicenceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Computers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AudienceId = table.Column<int>(type: "int", nullable: true),
                    IpAdress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Videocard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RAM = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalSpace = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Computers_Audiences_AudienceId",
                        column: x => x.AudienceId,
                        principalTable: "Audiences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Licences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    LicenceTypeId = table.Column<int>(type: "int", nullable: true),
                    LicenceDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licences_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Licences_LicenceDetailses_LicenceDetailsId",
                        column: x => x.LicenceDetailsId,
                        principalTable: "LicenceDetailses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Licences_LicenceType_LicenceTypeId",
                        column: x => x.LicenceTypeId,
                        principalTable: "LicenceType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SoftwareTechnicalDetailses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjectAreaId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiredSpace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QRURL = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoftwareTechnicalDetailses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoftwareTechnicalDetailses_SubjectAreas_SubjectAreaId",
                        column: x => x.SubjectAreaId,
                        principalTable: "SubjectAreas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Softwares",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoftwareTechnicalDetailsId = table.Column<int>(type: "int", nullable: false),
                    LicenceId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Softwares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Softwares_Licences_LicenceId",
                        column: x => x.LicenceId,
                        principalTable: "Licences",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Softwares_SoftwareTechnicalDetailses_SoftwareTechnicalDetailsId",
                        column: x => x.SoftwareTechnicalDetailsId,
                        principalTable: "SoftwareTechnicalDetailses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComputerSoftware",
                columns: table => new
                {
                    ComputersId = table.Column<int>(type: "int", nullable: false),
                    SoftwaresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerSoftware", x => new { x.ComputersId, x.SoftwaresId });
                    table.ForeignKey(
                        name: "FK_ComputerSoftware_Computers_ComputersId",
                        column: x => x.ComputersId,
                        principalTable: "Computers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerSoftware_Softwares_SoftwaresId",
                        column: x => x.SoftwaresId,
                        principalTable: "Softwares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Computers_AudienceId",
                table: "Computers",
                column: "AudienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerSoftware_SoftwaresId",
                table: "ComputerSoftware",
                column: "SoftwaresId");

            migrationBuilder.CreateIndex(
                name: "IX_Licences_EmployeeId",
                table: "Licences",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Licences_LicenceDetailsId",
                table: "Licences",
                column: "LicenceDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Licences_LicenceTypeId",
                table: "Licences",
                column: "LicenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_LicenceId",
                table: "Softwares",
                column: "LicenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Softwares_SoftwareTechnicalDetailsId",
                table: "Softwares",
                column: "SoftwareTechnicalDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_SoftwareTechnicalDetailses_SubjectAreaId",
                table: "SoftwareTechnicalDetailses",
                column: "SubjectAreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerSoftware");

            migrationBuilder.DropTable(
                name: "Computers");

            migrationBuilder.DropTable(
                name: "Softwares");

            migrationBuilder.DropTable(
                name: "Audiences");

            migrationBuilder.DropTable(
                name: "Licences");

            migrationBuilder.DropTable(
                name: "SoftwareTechnicalDetailses");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "LicenceDetailses");

            migrationBuilder.DropTable(
                name: "LicenceType");

            migrationBuilder.DropTable(
                name: "SubjectAreas");
        }
    }
}
