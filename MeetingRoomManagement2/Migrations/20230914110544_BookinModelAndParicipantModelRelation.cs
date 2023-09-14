using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingRoomManagement2.Migrations
{
    /// <inheritdoc />
    public partial class BookinModelAndParicipantModelRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Participants_BookingId",
                table: "Participants",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_Bookings_BookingId",
                table: "Participants",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_Bookings_BookingId",
                table: "Participants");

            migrationBuilder.DropIndex(
                name: "IX_Participants_BookingId",
                table: "Participants");
        }
    }
}
