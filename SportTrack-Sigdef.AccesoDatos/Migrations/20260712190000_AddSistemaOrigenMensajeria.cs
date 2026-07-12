using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260712190000_AddSistemaOrigenMensajeria")]
    public partial class AddSistemaOrigenMensajeria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SistemaOrigen",
                schema: "comunicacion",
                table: "Hilos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "sporttrack");

            migrationBuilder.AddColumn<string>(
                name: "SistemaOrigen",
                schema: "comunicacion",
                table: "CampanasEnvio",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "sporttrack");

            migrationBuilder.Sql(
                @"UPDATE comunicacion.""Hilos"" SET ""SistemaOrigen"" = 'sporttrack' WHERE ""SistemaOrigen"" IS NULL OR ""SistemaOrigen"" = '';");
            migrationBuilder.Sql(
                @"UPDATE comunicacion.""CampanasEnvio"" SET ""SistemaOrigen"" = 'sporttrack' WHERE ""SistemaOrigen"" IS NULL OR ""SistemaOrigen"" = '';");

            migrationBuilder.CreateIndex(
                name: "IX_Hilos_SistemaOrigen",
                schema: "comunicacion",
                table: "Hilos",
                column: "SistemaOrigen");

            migrationBuilder.CreateIndex(
                name: "IX_Campanas_SistemaOrigen",
                schema: "comunicacion",
                table: "CampanasEnvio",
                column: "SistemaOrigen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Hilos_SistemaOrigen",
                schema: "comunicacion",
                table: "Hilos");

            migrationBuilder.DropIndex(
                name: "IX_Campanas_SistemaOrigen",
                schema: "comunicacion",
                table: "CampanasEnvio");

            migrationBuilder.DropColumn(
                name: "SistemaOrigen",
                schema: "comunicacion",
                table: "Hilos");

            migrationBuilder.DropColumn(
                name: "SistemaOrigen",
                schema: "comunicacion",
                table: "CampanasEnvio");
        }
    }
}
