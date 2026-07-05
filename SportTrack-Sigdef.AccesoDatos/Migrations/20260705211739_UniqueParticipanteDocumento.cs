using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class UniqueParticipanteDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Participantes_Documento",
                schema: "regatas",
                table: "Participantes",
                column: "Documento",
                unique: true,
                filter: "\"Documento\" IS NOT NULL AND \"Documento\" <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Participantes_Documento",
                schema: "regatas",
                table: "Participantes");
        }
    }
}
