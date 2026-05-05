using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballHub.Migrations
{
    /// <inheritdoc />
    public partial class initialcomponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataRegistrazione = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pronostici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    SquadraCasa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SquadraOspite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RisultatoPronosticato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataPartita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataInserimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pronostici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pronostici_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SquadrePreferite",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    ApiSquadraId = table.Column<int>(type: "int", nullable: false),
                    NomeSquadra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Campionato = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SquadrePreferite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SquadrePreferite_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pronostici_UtenteId",
                table: "Pronostici",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_SquadrePreferite_UtenteId",
                table: "SquadrePreferite",
                column: "UtenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pronostici");

            migrationBuilder.DropTable(
                name: "SquadrePreferite");

            migrationBuilder.DropTable(
                name: "Utenti");
        }
    }
}
