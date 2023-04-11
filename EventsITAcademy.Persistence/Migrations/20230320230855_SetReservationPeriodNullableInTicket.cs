using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsITAcademy.Persistence.Migrations
{
    public partial class SetReservationPeriodNullableInTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditable",
                table: "Events");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReservationDeadline",
                table: "Tickets",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ReservationDeadline",
                table: "Tickets",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEditable",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
