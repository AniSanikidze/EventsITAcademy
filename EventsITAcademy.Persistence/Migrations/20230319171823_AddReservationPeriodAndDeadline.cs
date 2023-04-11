using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsITAcademy.Persistence.Migrations
{
    public partial class AddReservationPeriodAndDeadline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationPeriodInMinutes",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDeadline",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ReservationPeriod",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDeadline",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ReservationPeriod",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "ReservationPeriodInMinutes",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
