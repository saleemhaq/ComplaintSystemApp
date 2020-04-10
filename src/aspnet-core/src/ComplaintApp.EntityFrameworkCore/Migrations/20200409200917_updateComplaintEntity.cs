using Microsoft.EntityFrameworkCore.Migrations;

namespace ComplaintApp.EntityFrameworkCore.Migrations
{
    public partial class updateComplaintEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplainantName",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ComplaintName",
                table: "Complaints",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComplaintName",
                table: "Complaints");

            migrationBuilder.AddColumn<string>(
                name: "ComplainantName",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
