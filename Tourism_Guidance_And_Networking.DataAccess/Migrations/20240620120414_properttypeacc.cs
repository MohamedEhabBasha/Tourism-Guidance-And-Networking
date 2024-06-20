using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class properttypeacc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyType",
                table: "Accommodations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Accommodations");
        }
    }
}
