using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventManager.Data.Migrations
{
    public partial class fourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
