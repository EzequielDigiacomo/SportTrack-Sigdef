using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddFichajeHabilitacionYDocs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                schema: "federacion",
                table: "Entrenadores",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaBaja",
                schema: "federacion",
                table: "Entrenadores",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoBaja",
                schema: "federacion",
                table: "Entrenadores",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoRevision",
                schema: "federacion",
                table: "DocumentacionPersonas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                schema: "federacion",
                table: "DocumentacionPersonas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observacion",
                schema: "federacion",
                table: "DocumentacionPersonas",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstadoFichaje",
                schema: "federacion",
                table: "AtletasFederados",
                type: "integer",
                nullable: false,
                defaultValue: 2);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaEnvioFichaje",
                schema: "federacion",
                table: "AtletasFederados",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRevision",
                schema: "federacion",
                table: "AtletasFederados",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimientoApto",
                schema: "federacion",
                table: "AtletasFederados",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoRevision",
                schema: "federacion",
                table: "AtletasFederados",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RevisadoPorUserId",
                schema: "federacion",
                table: "AtletasFederados",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FichajeRevisionHistorial",
                schema: "federacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParticipanteId = table.Column<int>(type: "integer", nullable: false),
                    EstadoAnterior = table.Column<int>(type: "integer", nullable: false),
                    EstadoNuevo = table.Column<int>(type: "integer", nullable: false),
                    Motivo = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RevisadoPorUserId = table.Column<int>(type: "integer", nullable: true),
                    RevisadoPorUsername = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FichajeRevisionHistorial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FichajeRevisionHistorial_AtletasFederados_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalSchema: "federacion",
                        principalTable: "AtletasFederados",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FichajeRevisionHistorial_ParticipanteId",
                schema: "federacion",
                table: "FichajeRevisionHistorial",
                column: "ParticipanteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FichajeRevisionHistorial",
                schema: "federacion");

            migrationBuilder.DropColumn(
                name: "Activo",
                schema: "federacion",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "FechaBaja",
                schema: "federacion",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "MotivoBaja",
                schema: "federacion",
                table: "Entrenadores");

            migrationBuilder.DropColumn(
                name: "EstadoRevision",
                schema: "federacion",
                table: "DocumentacionPersonas");

            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                schema: "federacion",
                table: "DocumentacionPersonas");

            migrationBuilder.DropColumn(
                name: "Observacion",
                schema: "federacion",
                table: "DocumentacionPersonas");

            migrationBuilder.DropColumn(
                name: "EstadoFichaje",
                schema: "federacion",
                table: "AtletasFederados");

            migrationBuilder.DropColumn(
                name: "FechaEnvioFichaje",
                schema: "federacion",
                table: "AtletasFederados");

            migrationBuilder.DropColumn(
                name: "FechaRevision",
                schema: "federacion",
                table: "AtletasFederados");

            migrationBuilder.DropColumn(
                name: "FechaVencimientoApto",
                schema: "federacion",
                table: "AtletasFederados");

            migrationBuilder.DropColumn(
                name: "MotivoRevision",
                schema: "federacion",
                table: "AtletasFederados");

            migrationBuilder.DropColumn(
                name: "RevisadoPorUserId",
                schema: "federacion",
                table: "AtletasFederados");
        }
    }
}
