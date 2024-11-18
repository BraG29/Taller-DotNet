﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quality_Management.DataAccess;

#nullable disable

namespace Quality_Management.Migrations
{
    [DbContext(typeof(QualityManagementDbContext))]
    [Migration("20241118232706_Create Quality_Managment_DB table ")]
    partial class CreateQuality_Managment_DBtable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Quality_Management.Model.Office", b =>
                {
                    b.Property<string>("OfficeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PositionsAmount")
                        .HasColumnType("int");

                    b.HasKey("OfficeId");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("Quality_Management.Model.Procedure", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("PlaceNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("place_number");

                    b.Property<DateTime>("ProcedureEnd")
                        .HasColumnType("datetime2")
                        .HasColumnName("procedure_end");

                    b.Property<DateTime>("ProcedureStart")
                        .HasColumnType("datetime2")
                        .HasColumnName("procedure_start");

                    b.Property<string>("WaitTime")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("wait_time");

                    b.Property<string>("office")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("office");

                    b.ToTable("Procedures");
                });

            modelBuilder.Entity("Quality_Management.Model.Procedure", b =>
                {
                    b.HasOne("Quality_Management.Model.Office", "Office")
                        .WithMany("Procedures")
                        .HasForeignKey("office")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Office");
                });

            modelBuilder.Entity("Quality_Management.Model.Office", b =>
                {
                    b.Navigation("Procedures");
                });
#pragma warning restore 612, 618
        }
    }
}
