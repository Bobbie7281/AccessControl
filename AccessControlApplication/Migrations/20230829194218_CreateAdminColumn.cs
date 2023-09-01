using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessControlApplication.Migrations
{
    /// <inheritdoc />
    public partial class CreateAdminColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Administrator",
                table: "UserDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Administrator",
                table: "UserDetails");
        }
    }
}
