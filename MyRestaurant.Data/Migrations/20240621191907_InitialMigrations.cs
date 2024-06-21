using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyRestaurant.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Size = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientsGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Size = table.Column<int>(type: "integer", nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientsGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientsGroups_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccommodatedClientsGroups",
                columns: table => new
                {
                    TableId = table.Column<int>(type: "integer", nullable: false),
                    ClientsGroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodatedClientsGroups", x => new { x.TableId, x.ClientsGroupId });
                    table.ForeignKey(
                        name: "FK_AccommodatedClientsGroups_ClientsGroups_ClientsGroupId",
                        column: x => x.ClientsGroupId,
                        principalTable: "ClientsGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccommodatedClientsGroups_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QueuedClientsGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientsGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    QueuedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueuedClientsGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueuedClientsGroups_ClientsGroups_ClientsGroupId",
                        column: x => x.ClientsGroupId,
                        principalTable: "ClientsGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodatedClientsGroups_ClientsGroupId",
                table: "AccommodatedClientsGroups",
                column: "ClientsGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientsGroups_CustomerId",
                table: "ClientsGroups",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_QueuedClientsGroups_ClientsGroupId",
                table: "QueuedClientsGroups",
                column: "ClientsGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodatedClientsGroups");

            migrationBuilder.DropTable(
                name: "QueuedClientsGroups");

            migrationBuilder.DropTable(
                name: "Tables");

            migrationBuilder.DropTable(
                name: "ClientsGroups");

            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
