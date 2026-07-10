using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddCampanasEnvio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampanasEnvio",
                schema: "comunicacion",
                columns: table => new
                {
                    IdCampana = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RemitenteId = table.Column<int>(type: "integer", nullable: false),
                    Asunto = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Cuerpo = table.Column<string>(type: "text", nullable: false),
                    EnviadoEn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CantidadDestinatarios = table.Column<int>(type: "integer", nullable: false),
                    TipoCampana = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampanasEnvio", x => x.IdCampana);
                    table.ForeignKey(
                        name: "FK_Campanas_Remitente",
                        column: x => x.RemitenteId,
                        principalSchema: "seguridad",
                        principalTable: "Usuarios",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hilos_IdCampana",
                schema: "comunicacion",
                table: "Hilos",
                column: "IdCampana");

            migrationBuilder.CreateIndex(
                name: "IX_Campanas_EnviadoEn",
                schema: "comunicacion",
                table: "CampanasEnvio",
                column: "EnviadoEn");

            migrationBuilder.CreateIndex(
                name: "IX_Campanas_RemitenteId",
                schema: "comunicacion",
                table: "CampanasEnvio",
                column: "RemitenteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hilos_Campanas",
                schema: "comunicacion",
                table: "Hilos",
                column: "IdCampana",
                principalSchema: "comunicacion",
                principalTable: "CampanasEnvio",
                principalColumn: "IdCampana",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hilos_Campanas",
                schema: "comunicacion",
                table: "Hilos");

            migrationBuilder.DropTable(
                name: "CampanasEnvio",
                schema: "comunicacion");

            migrationBuilder.DropIndex(
                name: "IX_Hilos_IdCampana",
                schema: "comunicacion",
                table: "Hilos");
        }
    }
}
