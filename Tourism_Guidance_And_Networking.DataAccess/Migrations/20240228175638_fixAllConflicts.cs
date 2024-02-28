using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Guidance_And_Networking.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class fixAllConflicts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodation_Companies_CompanyId",
                table: "Accommodation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Accommodation_AccommodationId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accommodation",
                table: "Accommodation");

            migrationBuilder.RenameTable(
                name: "Accommodation",
                newName: "Accommodations");

            migrationBuilder.RenameIndex(
                name: "IX_Accommodation_CompanyId",
                table: "Accommodations",
                newName: "IX_Accommodations_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accommodations",
                table: "Accommodations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BookingHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BookingTotalPrice = table.Column<double>(type: "float", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingHeaders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingHeaderId = table.Column<int>(type: "int", nullable: false),
                    AccommodationId = table.Column<int>(type: "int", nullable: true),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Accommodations_AccommodationId",
                        column: x => x.AccommodationId,
                        principalTable: "Accommodations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BookingDetails_BookingHeaders_BookingHeaderId",
                        column: x => x.BookingHeaderId,
                        principalTable: "BookingHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingDetails_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_AccommodationId",
                table: "BookingDetails",
                column: "AccommodationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_BookingHeaderId",
                table: "BookingDetails",
                column: "BookingHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingDetails_RoomId",
                table: "BookingDetails",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingHeaders_ApplicationUserId",
                table: "BookingHeaders",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Companies_CompanyId",
                table: "Accommodations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Accommodations_AccommodationId",
                table: "Reservations",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Companies_CompanyId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Accommodations_AccommodationId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "BookingDetails");

            migrationBuilder.DropTable(
                name: "BookingHeaders");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Accommodation_AccommodationId",
                table: "Reservations",
                column: "AccommodationId",
                principalTable: "Accommodation",
                principalColumn: "Id");
        }
    }
}
