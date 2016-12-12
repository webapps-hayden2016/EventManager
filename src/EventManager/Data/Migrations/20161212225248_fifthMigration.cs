using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManager.Data.Migrations
{
    public partial class fifthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_AspNetUsers_ApplicationUserId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ApplicationUserId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "UserEvents",
                columns: table => new
                {
                    EventID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEvents", x => new { x.EventID, x.UserID });
                    table.ForeignKey(
                        name: "FK_UserEvents_Events_EventID",
                        column: x => x.EventID,
                        principalTable: "Events",
                        principalColumn: "EventID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserEvents_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_EventID",
                table: "UserEvents",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_UserID",
                table: "UserEvents",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEvents");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ApplicationUserId",
                table: "Events",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_AspNetUsers_ApplicationUserId",
                table: "Events",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
