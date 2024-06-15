using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addForienKeyInHotelForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Hotels",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_ApplicationUserId",
                table: "Hotels",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hotels_AspNetUsers_ApplicationUserId",
                table: "Hotels",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hotels_AspNetUsers_ApplicationUserId",
                table: "Hotels");

            migrationBuilder.DropIndex(
                name: "IX_Hotels_ApplicationUserId",
                table: "Hotels");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Hotels");
        }
    }
}
