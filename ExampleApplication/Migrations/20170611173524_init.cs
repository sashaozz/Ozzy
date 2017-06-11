using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExampleApplication.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoanApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Amount = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    From = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    WelcomeMessageSent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainEvents",
                columns: table => new
                {
                    Sequence = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventData = table.Column<byte[]>(nullable: true),
                    EventType = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEvents", x => x.Sequence);
                });

            migrationBuilder.CreateTable(
                name: "DistributedLocks",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    LockDateTime = table.Column<DateTime>(nullable: false),
                    LockId = table.Column<Guid>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributedLocks", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Sagas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SagaState = table.Column<byte[]>(nullable: true),
                    SagaVersion = table.Column<int>(nullable: false),
                    StateType = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sagas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sequences",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Number = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequences", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "FeatureFlags",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    SerializedConfiguration = table.Column<byte[]>(nullable: true),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureFlags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Queues",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Payload = table.Column<byte[]>(nullable: true),
                    QueueName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SagaCorrelationIds",
                columns: table => new
                {
                    PropertyName = table.Column<string>(nullable: false),
                    SagaType = table.Column<string>(nullable: false),
                    SagaId = table.Column<Guid>(nullable: false),
                    PropertyValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SagaCorrelationIds", x => new { x.PropertyName, x.SagaType, x.SagaId });
                    table.ForeignKey(
                        name: "FK_SagaCorrelationIds_Sagas_SagaId",
                        column: x => x.SagaId,
                        principalTable: "Sagas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SagaCorrelationIds_SagaId",
                table: "SagaCorrelationIds",
                column: "SagaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanApplications");

            migrationBuilder.DropTable(
                name: "DomainEvents");

            migrationBuilder.DropTable(
                name: "DistributedLocks");

            migrationBuilder.DropTable(
                name: "SagaCorrelationIds");

            migrationBuilder.DropTable(
                name: "Sequences");

            migrationBuilder.DropTable(
                name: "FeatureFlags");

            migrationBuilder.DropTable(
                name: "Queues");

            migrationBuilder.DropTable(
                name: "Sagas");
        }
    }
}
