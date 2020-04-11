using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComplaintApp.EntityFrameworkCore.Migrations
{
    public partial class AddEntityAuditTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditID = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IPAddress = table.Column<string>(nullable: true),
                    AreaAccessed = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditTrails");
        }
    }
}
