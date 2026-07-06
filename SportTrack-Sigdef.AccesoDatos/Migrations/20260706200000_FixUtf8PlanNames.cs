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
                UPDATE catalogos."PlanesSaaS" SET "Nombre" = 'Pack Dúo (S)' WHERE "Id" = 7;
                UPDATE catalogos."PlanesSaaS" SET "Nombre" = 'Pack Dúo (M)' WHERE "Id" = 8;
                UPDATE catalogos."PlanesSaaS" SET "Nombre" = 'Pack Dúo (L)' WHERE "Id" = 9;
                UPDATE catalogos."PlanesSaaS" SET "Nombre" = 'Pack Dúo (S)' WHERE "Nombre" LIKE 'Pack D%o (S)';
                UPDATE catalogos."PlanesSaaS" SET "Nombre" = 'Pack Dúo (M)' WHERE "Nombre" LIKE 'Pack D%o (M)';
                UPDATE catalogos."PlanesSaaS" SET "Nombre" = 'Pack Dúo (L)' WHERE "Nombre" LIKE 'Pack D%o (L)';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No revertir: los nombres corruptos no deben restaurarse
        }
    }
}
