using Microsoft.EntityFrameworkCore.Migrations;

namespace ComplaintApp.EntityFrameworkCore.Migrations
{
    public partial class updateComplaintEntityAddedCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Complaints",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Complaints",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Complaints");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Complaints");
        }
    }
}
