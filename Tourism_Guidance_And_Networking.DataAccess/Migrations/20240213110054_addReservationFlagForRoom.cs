using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addReservationFlagForRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReserved",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReserved",
                table: "Rooms");
        }
    }
}
