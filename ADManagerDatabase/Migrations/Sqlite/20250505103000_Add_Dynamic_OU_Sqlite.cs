using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ADManager.Database.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class Add_Dynamic_OU_Sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseDynamicOU",
                table: "PermissionDelegate",
                type: "INTEGER",
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
