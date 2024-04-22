using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addCountOfReservedToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfReserved",
                table: "Accommodations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfReserved",
                table: "Accommodations");
        }
    }
}
