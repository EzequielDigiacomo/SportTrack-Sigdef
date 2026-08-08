using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;
using System;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260808160000_AddGrupoLargadaId")]
    public partial class AddGrupoLargadaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GrupoLargadaId",
                schema: "regatas",
                table: "EventoPruebas",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventoPruebas_GrupoLargadaId",
                schema: "regatas",
                table: "EventoPruebas",
                column: "GrupoLargadaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventoPruebas_GrupoLargadaId",
                schema: "regatas",
                table: "EventoPruebas");

            migrationBuilder.DropColumn(
                name: "GrupoLargadaId",
                schema: "regatas",
                table: "EventoPruebas");
        }
    }
}
