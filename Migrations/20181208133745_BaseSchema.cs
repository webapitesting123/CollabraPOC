using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FabrikamResidencesActivities.Migrations
{
    public partial class BaseSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PortalActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                     table.PrimaryKey("PK_PortalActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attendee",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PortalActivityId = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendee_PortalActivity_PortalActivityId",
                        column: x => x.PortalActivityId,
                        principalTable: "PortalActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PortalActivity",
                columns: new[] { "Id", "Date", "Description", "Name" },
                values: new object[] { 1, new DateTime(2018, 12, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), "Come join us for an exciting game of Bingo with great prizes.", "Bingo" });

            migrationBuilder.InsertData(
                table: "PortalActivity",
                columns: new[] { "Id", "Date", "Description", "Name" },
                values: new object[] { 2, new DateTime(2018, 12, 13, 18, 0, 0, 0, DateTimeKind.Unspecified), "Meet us at the Shuffleboard court!", "Shuffleboard Competition" });

            migrationBuilder.InsertData(
                table: "Attendee",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "PortalActivityId" },
                values: new object[,]
                {
                     { 1, "Joe@Addict.com", "Joe", "Bingo", 1 },
                     { 2, "jdoe@anonymous.com", "john", "doe", 1 },
                     { 3, "champ@shuffleboard.com", "Jill", "Hill", 2 },
                     { 4, "jdoe@anonymous.com", "John", "Doe", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendee_PortalActivityId",
                table: "Attendee",
                column: "PortalActivityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendee");

            migrationBuilder.DropTable(
                name: "PortalActivity");
        }
    }
}
