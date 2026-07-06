using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class FixUtf8PlanNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE "PlanesSaaS" SET "Nombre" = 'Pack Dúo (S)' WHERE "Id" = 7;
                UPDATE "PlanesSaaS" SET "Nombre" = 'Pack Dúo (M)' WHERE "Id" = 8;
                UPDATE "PlanesSaaS" SET "Nombre" = 'Pack Dúo (L)' WHERE "Id" = 9;
                UPDATE "PlanesSaaS" SET "Nombre" = 'Pack Dúo (S)' WHERE "Nombre" LIKE 'Pack D%o (S)';
                UPDATE "PlanesSaaS" SET "Nombre" = 'Pack Dúo (M)' WHERE "Nombre" LIKE 'Pack D%o (M)';
                UPDATE "PlanesSaaS" SET "Nombre" = 'Pack Dúo (L)' WHERE "Nombre" LIKE 'Pack D%o (L)';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No revertir: los nombres corruptos no deben restaurarse
        }
    }
}
