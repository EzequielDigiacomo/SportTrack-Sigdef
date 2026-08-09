using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260809194500_AddPlanDescuentoAnual")]
    public partial class AddPlanDescuentoAnual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DescuentoAnualPorcentaje",
                schema: "catalogos",
                table: "PlanesSaaS",
                type: "numeric(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            // Si ya había PrecioAnual (p.ej. 10× mensual), deriva el % sobre 12× mensual
            migrationBuilder.Sql("""
                UPDATE catalogos."PlanesSaaS"
                SET
                    "DescuentoAnualPorcentaje" = CASE
                        WHEN "Precio" > 0 AND ("Precio" * 12) > 0 THEN
                            ROUND(
                                GREATEST(0, LEAST(100,
                                    (1 - ("PrecioAnual" / ("Precio" * 12))) * 100
                                ))::numeric,
                                2
                            )
                        ELSE 16.67
                    END,
                    "PrecioAnual" = ROUND(
                        "Precio" * 12 * (1 - (
                            CASE
                                WHEN "Precio" > 0 AND ("Precio" * 12) > 0 THEN
                                    GREATEST(0, LEAST(100,
                                        (1 - ("PrecioAnual" / ("Precio" * 12))) * 100
                                    ))
                                ELSE 16.67
                            END
                        ) / 100),
                        2
                    );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescuentoAnualPorcentaje",
                schema: "catalogos",
                table: "PlanesSaaS");
        }
    }
}
