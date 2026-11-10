using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddCambiosDelegadosYTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CambiosDelegadosHabilitados",
                schema: "regatas",
                table: "Eventos",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLimiteCambiosDelegados",
                schema: "regatas",
                table: "Eventos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TokensAccesoCambios",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IdEvento = table.Column<int>(type: "integer", nullable: false),
                    IdClub = table.Column<int>(type: "integer", nullable: false),
                    ExpiraEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revocado = table.Column<bool>(type: "boolean", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    CreadoPor = table.Column<string>(type: "text", nullable: true),
                    Usos = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokensAccesoCambios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TokensAccesoCambios_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TokensAccesoCambios_Eventos_IdEvento",
                        column: x => x.IdEvento,
                        principalSchema: "regatas",
                        principalTable: "Eventos",
                        principalColumn: "IdEvento",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokensAccesoCambios_Evento_Club",
                schema: "regatas",
                table: "TokensAccesoCambios",
                columns: new[] { "IdEvento", "IdClub" });

            migrationBuilder.CreateIndex(
                name: "IX_TokensAccesoCambios_IdClub",
                schema: "regatas",
                table: "TokensAccesoCambios",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_TokensAccesoCambios_Token",
                schema: "regatas",
                table: "TokensAccesoCambios",
                column: "Token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TokensAccesoCambios",
                schema: "regatas");

            migrationBuilder.DropColumn(
                name: "CambiosDelegadosHabilitados",
                schema: "regatas",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "FechaLimiteCambiosDelegados",
                schema: "regatas",
                table: "Eventos");
        }
    }
}
