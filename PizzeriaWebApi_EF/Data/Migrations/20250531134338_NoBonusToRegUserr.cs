using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzeriaWebApi_EF.Migrations.ApplicationUser
{
    /// <inheritdoc />
    public partial class NoBonusToRegUserr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BonusPoints",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusPoints",
                table: "AspNetUsers");
        }
    }
}
