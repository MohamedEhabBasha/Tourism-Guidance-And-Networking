using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMatrix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserMatrices",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMatrices", x => new { x.UserID, x.ItemID, x.Action });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMatrices");
        }
    }
}
