using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingRoomManagement2.Migrations
{
    /// <inheritdoc />
    public partial class BookingModel2Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingModel_Rooms_RoomModelId",
                table: "BookingModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookingModel",
                table: "BookingModel");

            migrationBuilder.RenameTable(
                name: "BookingModel",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_BookingModel_RoomModelId",
                table: "Bookings",
                newName: "IX_Bookings_RoomModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Rooms_RoomModelId",
                table: "Bookings",
                column: "RoomModelId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Rooms_RoomModelId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "BookingModel");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_RoomModelId",
                table: "BookingModel",
                newName: "IX_BookingModel_RoomModelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookingModel",
                table: "BookingModel",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingModel_Rooms_RoomModelId",
                table: "BookingModel",
                column: "RoomModelId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
