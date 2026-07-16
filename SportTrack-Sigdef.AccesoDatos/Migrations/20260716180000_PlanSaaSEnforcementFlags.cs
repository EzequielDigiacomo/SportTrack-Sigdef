using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260716180000_PlanSaaSEnforcementFlags")]
    public partial class PlanSaaSEnforcementFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccesoDashboardClub",
                schema: "catalogos",
                table: "PlanesSaaS",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PermitirCargaImagenes",
                schema: "catalogos",
                table: "PlanesSaaS",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ExportacionPdf",
                schema: "catalogos",
                table: "PlanesSaaS",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            // Matriz producto: MaxAtletas 200/400/-1 + flags
            migrationBuilder.Sql(@"
UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = 200,
    ""ResultadosTiempoReal"" = FALSE,
    ""ExportacionExcel"" = FALSE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = FALSE,
    ""AccesoDashboardClub"" = FALSE,
    ""PermitirCargaImagenes"" = FALSE
WHERE ""Id"" = 1;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = 400,
    ""ResultadosTiempoReal"" = FALSE,
    ""ExportacionExcel"" = FALSE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = FALSE,
    ""AccesoDashboardClub"" = TRUE,
    ""PermitirCargaImagenes"" = FALSE
WHERE ""Id"" = 2;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = -1,
    ""ResultadosTiempoReal"" = FALSE,
    ""ExportacionExcel"" = TRUE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = TRUE,
    ""AccesoDashboardClub"" = TRUE,
    ""PermitirCargaImagenes"" = TRUE
WHERE ""Id"" = 3;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = 200,
    ""ResultadosTiempoReal"" = FALSE,
    ""ExportacionExcel"" = FALSE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = FALSE,
    ""AccesoDashboardClub"" = FALSE,
    ""PermitirCargaImagenes"" = FALSE
WHERE ""Id"" = 4;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = 400,
    ""ResultadosTiempoReal"" = TRUE,
    ""ExportacionExcel"" = FALSE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = FALSE,
    ""AccesoDashboardClub"" = FALSE,
    ""PermitirCargaImagenes"" = FALSE
WHERE ""Id"" = 5;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = -1,
    ""ResultadosTiempoReal"" = TRUE,
    ""ExportacionExcel"" = TRUE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = TRUE,
    ""AccesoDashboardClub"" = FALSE,
    ""PermitirCargaImagenes"" = FALSE
WHERE ""Id"" = 6;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = 200,
    ""ResultadosTiempoReal"" = FALSE,
    ""ExportacionExcel"" = TRUE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = TRUE,
    ""AccesoDashboardClub"" = FALSE,
    ""PermitirCargaImagenes"" = FALSE
WHERE ""Id"" = 7;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = 400,
    ""ResultadosTiempoReal"" = TRUE,
    ""ExportacionExcel"" = TRUE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = TRUE,
    ""AccesoDashboardClub"" = TRUE,
    ""PermitirCargaImagenes"" = FALSE
WHERE ""Id"" = 8;

UPDATE catalogos.""PlanesSaaS"" SET
    ""MaxAtletas"" = -1,
    ""ResultadosTiempoReal"" = TRUE,
    ""ExportacionExcel"" = TRUE,
    ""ExportacionPdf"" = TRUE,
    ""SoportePrioritario"" = TRUE,
    ""AccesoDashboardClub"" = TRUE,
    ""PermitirCargaImagenes"" = TRUE
WHERE ""Id"" = 9;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccesoDashboardClub",
                schema: "catalogos",
                table: "PlanesSaaS");

            migrationBuilder.DropColumn(
                name: "PermitirCargaImagenes",
                schema: "catalogos",
                table: "PlanesSaaS");

            migrationBuilder.DropColumn(
                name: "ExportacionPdf",
                schema: "catalogos",
                table: "PlanesSaaS");
        }
    }
}
