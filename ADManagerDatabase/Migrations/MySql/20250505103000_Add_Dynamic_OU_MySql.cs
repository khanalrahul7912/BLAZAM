using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADManager.Database.Migrations.MySql
{
    /// <inheritdoc />
    public partial class Add_Dynamic_OU_MySql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseDynamicOU",
                table: "PermissionDelegate",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseDynamicOU",
                table: "PermissionDelegate");
        }
    }
}
