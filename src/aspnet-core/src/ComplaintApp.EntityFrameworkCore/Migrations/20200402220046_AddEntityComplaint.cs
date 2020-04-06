using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComplaintApp.EntityFrameworkCore.Migrations
{
    public partial class AddEntityComplaint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComplainantName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<int>(nullable: false),
                    UpdatedTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<int>(nullable: false),
                    ComplaintRegarding = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Complaints");
        }
    }
}
