﻿// <auto-generated />
using System;
using AccountingSoftware.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountingSoftware.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20230419095110_ChangeName")]
    partial class ChangeName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AccountingSoftware.Models.Audience", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Audiences");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Computer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AudienceId")
                        .HasColumnType("int");

                    b.Property<string>("IpAdress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Processor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RAM")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TotalSpace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Videocard")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AudienceId");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Licence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("LicenceDetailsId")
                        .HasColumnType("int");

                    b.Property<int?>("LicenceTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LicenceDetailsId");

                    b.HasIndex("LicenceTypeId");

                    b.ToTable("Licences");
                });

            modelBuilder.Entity("AccountingSoftware.Models.LicenceDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("LicenceDetailses");
                });

            modelBuilder.Entity("AccountingSoftware.Models.LicenceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LicenceType");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Software", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("LicenceId")
                        .HasColumnType("int");

                    b.Property<int>("SoftwareTechnicalDetailsId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LicenceId");

                    b.HasIndex("SoftwareTechnicalDetailsId");

                    b.ToTable("Softwares");
                });

            modelBuilder.Entity("AccountingSoftware.Models.SoftwareTechnicalDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequiredSpace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SubjectAreaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SubjectAreaId");

                    b.ToTable("SoftwareTechnicalDetailses");
                });

            modelBuilder.Entity("AccountingSoftware.Models.SubjectArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SubjectAreas");
                });

            modelBuilder.Entity("ComputerSoftware", b =>
                {
                    b.Property<int>("ComputersId")
                        .HasColumnType("int");

                    b.Property<int>("SoftwaresId")
                        .HasColumnType("int");

                    b.HasKey("ComputersId", "SoftwaresId");

                    b.HasIndex("SoftwaresId");

                    b.ToTable("ComputerSoftware");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Computer", b =>
                {
                    b.HasOne("AccountingSoftware.Models.Audience", "Audience")
                        .WithMany("Computers")
                        .HasForeignKey("AudienceId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Audience");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Licence", b =>
                {
                    b.HasOne("AccountingSoftware.Models.Employee", "Employee")
                        .WithMany("Licence")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("AccountingSoftware.Models.LicenceDetails", "LicenceDetails")
                        .WithMany("Licences")
                        .HasForeignKey("LicenceDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountingSoftware.Models.LicenceType", "LicenceType")
                        .WithMany("Licences")
                        .HasForeignKey("LicenceTypeId");

                    b.Navigation("Employee");

                    b.Navigation("LicenceDetails");

                    b.Navigation("LicenceType");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Software", b =>
                {
                    b.HasOne("AccountingSoftware.Models.Licence", "Licence")
                        .WithMany("Softwares")
                        .HasForeignKey("LicenceId");

                    b.HasOne("AccountingSoftware.Models.SoftwareTechnicalDetails", "SoftwareTechnicalDetails")
                        .WithMany("Softwares")
                        .HasForeignKey("SoftwareTechnicalDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Licence");

                    b.Navigation("SoftwareTechnicalDetails");
                });

            modelBuilder.Entity("AccountingSoftware.Models.SoftwareTechnicalDetails", b =>
                {
                    b.HasOne("AccountingSoftware.Models.SubjectArea", "SubjectArea")
                        .WithMany("SoftwareTechnicalDetailses")
                        .HasForeignKey("SubjectAreaId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("SubjectArea");
                });

            modelBuilder.Entity("ComputerSoftware", b =>
                {
                    b.HasOne("AccountingSoftware.Models.Computer", null)
                        .WithMany()
                        .HasForeignKey("ComputersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountingSoftware.Models.Software", null)
                        .WithMany()
                        .HasForeignKey("SoftwaresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccountingSoftware.Models.Audience", b =>
                {
                    b.Navigation("Computers");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Employee", b =>
                {
                    b.Navigation("Licence");
                });

            modelBuilder.Entity("AccountingSoftware.Models.Licence", b =>
                {
                    b.Navigation("Softwares");
                });

            modelBuilder.Entity("AccountingSoftware.Models.LicenceDetails", b =>
                {
                    b.Navigation("Licences");
                });

            modelBuilder.Entity("AccountingSoftware.Models.LicenceType", b =>
                {
                    b.Navigation("Licences");
                });

            modelBuilder.Entity("AccountingSoftware.Models.SoftwareTechnicalDetails", b =>
                {
                    b.Navigation("Softwares");
                });

            modelBuilder.Entity("AccountingSoftware.Models.SubjectArea", b =>
                {
                    b.Navigation("SoftwareTechnicalDetailses");
                });
#pragma warning restore 612, 618
        }
    }
}