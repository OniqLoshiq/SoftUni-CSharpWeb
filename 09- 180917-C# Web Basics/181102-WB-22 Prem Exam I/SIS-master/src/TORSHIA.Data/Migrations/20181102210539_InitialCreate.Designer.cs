﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TORSHIA.Data;

namespace TORSHIA.Data.Migrations
{
    [DbContext(typeof(TorshiaDbContext))]
    [Migration("20181102210539_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TORSHIA.Models.Report", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ReportedOn");

                    b.Property<string>("ReporterId");

                    b.Property<int>("Status");

                    b.Property<string>("TaskId");

                    b.HasKey("Id");

                    b.HasIndex("ReporterId");

                    b.HasIndex("TaskId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("TORSHIA.Models.Sector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Sectors");
                });

            modelBuilder.Entity("TORSHIA.Models.Task", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("DueDate");

                    b.Property<bool>("IsReported");

                    b.Property<string>("Participants");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TORSHIA.Models.TaskSector", b =>
                {
                    b.Property<string>("TaskId");

                    b.Property<int>("SectorId");

                    b.HasKey("TaskId", "SectorId");

                    b.HasIndex("SectorId");

                    b.ToTable("TaskSectors");
                });

            modelBuilder.Entity("TORSHIA.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Password");

                    b.Property<string>("Role")
                        .IsRequired();

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TORSHIA.Models.Report", b =>
                {
                    b.HasOne("TORSHIA.Models.User", "Reporter")
                        .WithMany("Reports")
                        .HasForeignKey("ReporterId");

                    b.HasOne("TORSHIA.Models.Task", "Task")
                        .WithMany("Reports")
                        .HasForeignKey("TaskId");
                });

            modelBuilder.Entity("TORSHIA.Models.TaskSector", b =>
                {
                    b.HasOne("TORSHIA.Models.Sector", "Sector")
                        .WithMany("Tasks")
                        .HasForeignKey("SectorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TORSHIA.Models.Task", "Task")
                        .WithMany("Sectors")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
