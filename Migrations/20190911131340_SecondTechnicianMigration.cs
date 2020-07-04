using Microsoft.EntityFrameworkCore.Migrations;

namespace Technicians.Migrations
{
    public partial class SecondTechnicianMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Technician",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailAddress",
                table: "Technician",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
