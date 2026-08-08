using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260808170000_AddDistancia6000m")]
    public partial class AddDistancia6000m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO catalogos.""Distancias"" (""Id"", ""DistanciaRegata"", ""GapSugerido"")
SELECT 17, 17, 15
WHERE NOT EXISTS (
    SELECT 1 FROM catalogos.""Distancias"" WHERE ""Id"" = 17
);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM catalogos.""Distancias"" WHERE ""Id"" = 17;");
        }
    }
}
