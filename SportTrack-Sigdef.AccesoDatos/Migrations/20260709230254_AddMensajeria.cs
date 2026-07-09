using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddMensajeria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "comunicacion");

            migrationBuilder.CreateTable(
                name: "Hilos",
                schema: "comunicacion",
                columns: table => new
                {
                    IdHilo = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Asunto = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IdCampana = table.Column<int>(type: "integer", nullable: true),
                    CreadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UltimoMensajeEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hilos", x => x.IdHilo);
                });

            migrationBuilder.CreateTable(
                name: "Mensajes",
                schema: "comunicacion",
                columns: table => new
                {
                    IdMensaje = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HiloId = table.Column<int>(type: "integer", nullable: false),
                    RemitenteId = table.Column<int>(type: "integer", nullable: false),
                    DestinatarioId = table.Column<int>(type: "integer", nullable: false),
                    Cuerpo = table.Column<string>(type: "text", nullable: false),
                    EnviadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LeidoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EliminadoPorRemitente = table.Column<bool>(type: "boolean", nullable: false),
                    EliminadoPorDestinatario = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensajes", x => x.IdMensaje);
                    table.ForeignKey(
                        name: "FK_Mensajes_Destinatario",
                        column: x => x.DestinatarioId,
                        principalSchema: "seguridad",
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensajes_Hilos",
                        column: x => x.HiloId,
                        principalSchema: "comunicacion",
                        principalTable: "Hilos",
                        principalColumn: "IdHilo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Mensajes_Remitente",
                        column: x => x.RemitenteId,
                        principalSchema: "seguridad",
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hilos_UltimoMensajeEn",
                schema: "comunicacion",
                table: "Hilos",
                column: "UltimoMensajeEn");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_Destinatario_Leido",
                schema: "comunicacion",
                table: "Mensajes",
                columns: new[] { "DestinatarioId", "LeidoEn" });

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_DestinatarioId",
                schema: "comunicacion",
                table: "Mensajes",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_HiloId",
                schema: "comunicacion",
                table: "Mensajes",
                column: "HiloId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensajes_RemitenteId",
                schema: "comunicacion",
                table: "Mensajes",
                column: "RemitenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mensajes",
                schema: "comunicacion");

            migrationBuilder.DropTable(
                name: "Hilos",
                schema: "comunicacion");
        }
    }
}
