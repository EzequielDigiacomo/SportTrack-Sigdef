using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditoriaEventoScope : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEvento",
                table: "Auditoria",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEventoPrueba",
                table: "Auditoria",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_IdEvento_Fecha",
                table: "Auditoria",
                columns: new[] { "IdEvento", "Fecha" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Auditoria_IdEvento_Fecha",
                table: "Auditoria");

            migrationBuilder.DropColumn(
                name: "IdEventoPrueba",
                table: "Auditoria");

            migrationBuilder.DropColumn(
                name: "IdEvento",
                table: "Auditoria");
        }
    }
}
