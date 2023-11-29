using Microsoft.EntityFrameworkCore.Migrations;
using Tourism_Guidance_And_Networking.Core.Consts;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table:"AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[] {Guid.NewGuid().ToString(),Roles.Admin,Roles.Admin.ToUpper(),Guid.NewGuid().ToString() }
                );
			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
				values: new object[] { Guid.NewGuid().ToString(), Roles.Tourist, Roles.Tourist.ToUpper(), Guid.NewGuid().ToString() }
				);
			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
				values: new object[] { Guid.NewGuid().ToString(), Roles.Company, Roles.Company.ToUpper(), Guid.NewGuid().ToString() }
				);
			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
				values: new object[] { Guid.NewGuid().ToString(), Roles.Hotel, Roles.Hotel.ToUpper(), Guid.NewGuid().ToString() }
				);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
        }
    }
}
