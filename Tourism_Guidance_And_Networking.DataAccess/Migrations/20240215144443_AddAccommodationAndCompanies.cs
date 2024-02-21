using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddAccommodationAndCompanies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodation_Companies_CompanyId",
                table: "Accommodation");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_Accommodation",
            //    table: "Accommodation");

            migrationBuilder.RenameTable(
                name: "Accommodation",
                newName: "Accommodations");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodation_CompanyId",
                table: "Accommodations",
                newName: "IX_Accommodations_CompanyId");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_Accommodations",
            //    table: "Accommodations",
            //    column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Companies_CompanyId",
                table: "Accommodations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Companies_CompanyId",
                table: "Accommodations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations");

            migrationBuilder.RenameTable(
                name: "Accommodations",
                newName: "Accommodation");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodations_CompanyId",
                table: "Accommodation",
                newName: "IX_Accommodation_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodation",
                table: "Accommodation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodation_Companies_CompanyId",
                table: "Accommodation",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
