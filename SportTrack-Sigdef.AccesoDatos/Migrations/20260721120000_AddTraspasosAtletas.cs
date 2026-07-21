using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260721120000_AddTraspasosAtletas")]
    public partial class AddTraspasosAtletas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PeriodosTraspaso",
                schema: "federacion",
                columns: table => new
                {
                    IdPeriodoTraspaso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFederacion = table.Column<int>(type: "integer", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreadoPorUsuarioId = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodosTraspaso", x => x.IdPeriodoTraspaso);
                    table.ForeignKey(
                        name: "FK_PeriodosTraspaso_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudesTraspaso",
                schema: "federacion",
                columns: table => new
                {
                    IdSolicitudTraspaso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdFederacion = table.Column<int>(type: "integer", nullable: false),
                    ParticipanteId = table.Column<int>(type: "integer", nullable: false),
                    IdClubOrigen = table.Column<int>(type: "integer", nullable: false),
                    IdClubDestino = table.Column<int>(type: "integer", nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    MotivoSolicitud = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MotivoRechazo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    SolicitadoPorUsuarioId = table.Column<int>(type: "integer", nullable: true),
                    FechaSolicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaRespuestaOrigen = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaRespuestaFederacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaEjecucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AprobadoPorUsuarioId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudesTraspaso", x => x.IdSolicitudTraspaso);
                    table.ForeignKey(
                        name: "FK_SolicitudesTraspaso_Clubes_IdClubDestino",
                        column: x => x.IdClubDestino,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesTraspaso_Clubes_IdClubOrigen",
                        column: x => x.IdClubOrigen,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudesTraspaso_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitudesTraspaso_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosTraspaso_Federacion_Activo_Fechas",
                schema: "federacion",
                table: "PeriodosTraspaso",
                columns: new[] { "IdFederacion", "Activo", "FechaInicio", "FechaFin" });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTraspaso_Federacion",
                schema: "federacion",
                table: "SolicitudesTraspaso",
                column: "IdFederacion");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTraspaso_Participante_Estado",
                schema: "federacion",
                table: "SolicitudesTraspaso",
                columns: new[] { "ParticipanteId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTraspaso_IdClubDestino",
                schema: "federacion",
                table: "SolicitudesTraspaso",
                column: "IdClubDestino");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudesTraspaso_IdClubOrigen",
                schema: "federacion",
                table: "SolicitudesTraspaso",
                column: "IdClubOrigen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitudesTraspaso",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "PeriodosTraspaso",
                schema: "federacion");
        }
    }
}
