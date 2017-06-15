using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExampleApplication.Migrations
{
    public partial class queues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FetchedAt",
                table: "Queues",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "Queues",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FetchedAt",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "Queues");
        }
    }
}
