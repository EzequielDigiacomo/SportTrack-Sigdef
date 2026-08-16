using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260816210000_SolicitudTraspasoOrigenNullable")]
    public partial class SolicitudTraspasoOrigenNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdClubOrigen",
                schema: "federacion",
                table: "SolicitudesTraspaso",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE federacion."SolicitudesTraspaso"
                SET "IdClubOrigen" = "IdClubDestino"
                WHERE "IdClubOrigen" IS NULL;
                """);

            migrationBuilder.AlterColumn<int>(
                name: "IdClubOrigen",
                schema: "federacion",
                table: "SolicitudesTraspaso",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
