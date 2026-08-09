using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260809190000_RemovePlanTournamentLimits")]
    public partial class RemovePlanTournamentLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE catalogos."PlanesSaaS" SET "MaxTorneosActivos" = -1;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE catalogos."PlanesSaaS" SET "MaxTorneosActivos" = 5 WHERE "Id" IN (1, 4, 7);
                UPDATE catalogos."PlanesSaaS" SET "MaxTorneosActivos" = 20 WHERE "Id" IN (2, 5, 8);
                UPDATE catalogos."PlanesSaaS" SET "MaxTorneosActivos" = -1 WHERE "Id" IN (3, 6, 9);
                """);
        }
    }
}
