﻿// <auto-generated />
using System;
using FabrikamResidences_Activities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FabrikamResidencesActivities.Migrations
{
    [DbContext(typeof(PortalContext))]
    partial class PortalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("FabrikamResidences_Activities.Models.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PortalActivityId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PortalActivityId");

                    b.ToTable("Attendee");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "Joe@Addict.com",
                            FirstName = "Joe",
                            LastName = "Bingo",
                            PortalActivityId = 1
                        },
                        new
                        {
                            Id = 2,
                            Email = "jdoe@anonymous.com",
                            FirstName = "john",
                            LastName = "doe",
                            PortalActivityId = 1
                        },
                        new
                        {
                            Id = 3,
                            Email = "champ@shuffleboard.com",
                            FirstName = "Jill",
                            LastName = "Hill",
                            PortalActivityId = 2
                        },
                        new
                        {
                            Id = 4,
                            Email = "jdoe@anonymous.com",
                            FirstName = "John",
                            LastName = "Doe",
                            PortalActivityId = 2
                        });
                });

            modelBuilder.Entity("FabrikamResidences_Activities.Models.PortalActivity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PortalActivity");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateTime(2020, 11, 12, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Come join us for an exciting game of Bingo with great prizes.",
                            Name = "Bingo"
                        },
                        new
                        {
                            Id = 2,
                            Date = new DateTime(2020, 11, 15, 18, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Meet us at the Shuffleboard court!",
                            Name = "Shuffleboard Competition"
                        });
                });

            modelBuilder.Entity("FabrikamResidences_Activities.Models.Attendee", b =>
                {
                    b.HasOne("FabrikamResidences_Activities.Models.PortalActivity", null)
                        .WithMany("Attendees")
                        .HasForeignKey("PortalActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FabrikamResidences_Activities.Models.PortalActivity", b =>
                {
                    b.Navigation("Attendees");
                });
#pragma warning restore 612, 618
        }
    }
}
