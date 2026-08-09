using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260809193000_AddPlanPrecioAnual")]
    public partial class AddPlanPrecioAnual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrecioAnual",
                schema: "catalogos",
                table: "PlanesSaaS",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            // Default: 10× mensual (equivalente a ~2 meses bonificados)
            migrationBuilder.Sql("""
                UPDATE catalogos."PlanesSaaS"
                SET "PrecioAnual" = "Precio" * 10
                WHERE "PrecioAnual" = 0;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrecioAnual",
                schema: "catalogos",
                table: "PlanesSaaS");
        }
    }
}
