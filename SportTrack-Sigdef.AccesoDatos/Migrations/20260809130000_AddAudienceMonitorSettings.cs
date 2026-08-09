using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SportTrack_Sigdef.AccesoDatos;

#nullable disable

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    [DbContext(typeof(SportTrackDbContext))]
    [Migration("20260809130000_AddAudienceMonitorSettings")]
    public partial class AddAudienceMonitorSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE TABLE IF NOT EXISTS public.""AudienceMonitorSettings"" (
    ""Id"" integer PRIMARY KEY,
    ""SoftCapacity"" integer NOT NULL,
    ""PresetId"" character varying(40) NOT NULL,
    ""PlanLabel"" character varying(120) NOT NULL,
    ""UpdatedAtUtc"" timestamp with time zone NOT NULL
);

INSERT INTO public.""AudienceMonitorSettings"" (""Id"", ""SoftCapacity"", ""PresetId"", ""PlanLabel"", ""UpdatedAtUtc"")
SELECT 1, 200, 'starter', 'API Starter + DB Basic-1gb', NOW()
WHERE NOT EXISTS (
    SELECT 1 FROM public.""AudienceMonitorSettings"" WHERE ""Id"" = 1
);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TABLE IF EXISTS public.""AudienceMonitorSettings"";");
        }
    }
}
