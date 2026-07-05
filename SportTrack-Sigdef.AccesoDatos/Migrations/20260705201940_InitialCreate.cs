using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SportTrack_Sigdef.AccesoDatos.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "federacion");

            migrationBuilder.EnsureSchema(
                name: "catalogos");

            migrationBuilder.EnsureSchema(
                name: "regatas");

            migrationBuilder.EnsureSchema(
                name: "seguridad");

            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Accion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Detalle = table.Column<string>(type: "text", nullable: false),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    IP = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Modulo = table.Column<string>(type: "text", nullable: false),
                    UserAgent = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Botes",
                schema: "catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Botes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                schema: "catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EdadMin = table.Column<int>(type: "integer", nullable: true),
                    EdadMax = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Distancias",
                schema: "catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DistanciaRegata = table.Column<int>(type: "integer", nullable: false),
                    GapSugerido = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distancias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanesSaaS",
                schema: "catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    MaxAtletas = table.Column<int>(type: "integer", nullable: false),
                    MaxTorneosActivos = table.Column<int>(type: "integer", nullable: false),
                    ResultadosTiempoReal = table.Column<bool>(type: "boolean", nullable: false),
                    ExportacionExcel = table.Column<bool>(type: "boolean", nullable: false),
                    SoportePrioritario = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanesSaaS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "federacion",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Tipo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Sexos",
                schema: "catalogos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sexos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Federaciones",
                schema: "federacion",
                columns: table => new
                {
                    IdFederacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sigla = table.Column<string>(type: "text", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cuit = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BancoNombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TipoCuenta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumeroCuenta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TitularCuenta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailCobro = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PlanSaaSId = table.Column<int>(type: "integer", nullable: true),
                    FechaAltaPlan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaVencimientoPlan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FrecuenciaPago = table.Column<string>(type: "text", nullable: true),
                    BloqueadaPorFaltaDePago = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Federaciones", x => x.IdFederacion);
                    table.ForeignKey(
                        name: "FK_Federaciones_PlanesSaaS_PlanSaaSId",
                        column: x => x.PlanSaaSId,
                        principalSchema: "catalogos",
                        principalTable: "PlanesSaaS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Pruebas",
                schema: "regatas",
                columns: table => new
                {
                    IdPrueba = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TipoBote = table.Column<int>(type: "integer", nullable: false),
                    CategoriaEdad = table.Column<int>(type: "integer", nullable: false),
                    DistanciaId = table.Column<int>(type: "integer", nullable: false),
                    SexoCompetencia = table.Column<int>(type: "integer", nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pruebas", x => x.IdPrueba);
                    table.ForeignKey(
                        name: "FK_Pruebas_Botes",
                        column: x => x.TipoBote,
                        principalSchema: "catalogos",
                        principalTable: "Botes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pruebas_Categorias",
                        column: x => x.CategoriaEdad,
                        principalSchema: "catalogos",
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pruebas_Distancias",
                        column: x => x.DistanciaId,
                        principalSchema: "catalogos",
                        principalTable: "Distancias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pruebas_Sexos",
                        column: x => x.SexoCompetencia,
                        principalSchema: "catalogos",
                        principalTable: "Sexos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clubes",
                schema: "catalogos",
                columns: table => new
                {
                    IdClub = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Siglas = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Direccion = table.Column<string>(type: "text", nullable: true),
                    Ubicacion = table.Column<string>(type: "text", nullable: true),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    IdFederacion = table.Column<int>(type: "integer", nullable: true),
                    PlanSaaSId = table.Column<int>(type: "integer", nullable: true),
                    FrecuenciaPago = table.Column<string>(type: "text", nullable: true),
                    FechaAltaPlan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaVencimientoPlan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BloqueadoPorFaltaDePago = table.Column<bool>(type: "boolean", nullable: false),
                    PagoAfiliacionAlDia = table.Column<bool>(type: "boolean", nullable: false),
                    SolicitudPagoPendiente = table.Column<bool>(type: "boolean", nullable: false),
                    EstadoMatricula = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubes", x => x.IdClub);
                    table.ForeignKey(
                        name: "FK_Clubes_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clubes_PlanesSaaS_PlanSaaSId",
                        column: x => x.PlanSaaSId,
                        principalSchema: "catalogos",
                        principalTable: "PlanesSaaS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                schema: "regatas",
                columns: table => new
                {
                    IdEvento = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Fecha = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ubicacion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Programada"),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FechaFinInscripciones = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EstaActivo = table.Column<bool>(type: "boolean", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    TipoEvento = table.Column<string>(type: "text", nullable: false),
                    FechaInicioInscripciones = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ciudad = table.Column<string>(type: "text", nullable: true),
                    Provincia = table.Column<string>(type: "text", nullable: true),
                    PrecioBase = table.Column<decimal>(type: "numeric", nullable: false),
                    CupoMaximo = table.Column<int>(type: "integer", nullable: false),
                    TieneCronometraje = table.Column<bool>(type: "boolean", nullable: false),
                    RequiereCertificadoMedico = table.Column<bool>(type: "boolean", nullable: false),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    IdClub = table.Column<int>(type: "integer", nullable: true),
                    IdFederacion = table.Column<int>(type: "integer", nullable: true),
                    InscripcionesHabilitadas = table.Column<bool>(type: "boolean", nullable: false),
                    RestringirSoloCategoriaPropia = table.Column<bool>(type: "boolean", nullable: false),
                    PermitirSub23EnSenior = table.Column<bool>(type: "boolean", nullable: false),
                    PermitirMasterBajarASenior = table.Column<bool>(type: "boolean", nullable: false),
                    PermitirCompletarK4 = table.Column<bool>(type: "boolean", nullable: false),
                    LimitacionBotesAB = table.Column<bool>(type: "boolean", nullable: false),
                    HoraInicioEvento = table.Column<TimeSpan>(type: "interval", nullable: false),
                    CarrilesDisponibles = table.Column<int>(type: "integer", nullable: false),
                    PerfilTiempo = table.Column<int>(type: "integer", nullable: false),
                    HoraInicioReceso = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraFinReceso = table.Column<TimeSpan>(type: "interval", nullable: false),
                    SinReceso = table.Column<bool>(type: "boolean", nullable: false),
                    GapEntrePruebas = table.Column<int>(type: "integer", nullable: false),
                    PermitirCombinadas = table.Column<bool>(type: "boolean", nullable: false),
                    UsarGapVariable = table.Column<bool>(type: "boolean", nullable: false),
                    TimeZoneId = table.Column<string>(type: "text", nullable: false),
                    CategoriasHabilitadas = table.Column<string>(type: "text", nullable: true),
                    BotesHabilitados = table.Column<string>(type: "text", nullable: true),
                    DistanciasHabilitadas = table.Column<string>(type: "text", nullable: true),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.IdEvento);
                    table.ForeignKey(
                        name: "FK_Eventos_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Eventos_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Participantes",
                schema: "regatas",
                columns: table => new
                {
                    ParticipanteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    SexoId = table.Column<int>(type: "integer", nullable: false),
                    CategoriaId = table.Column<int>(type: "integer", nullable: true),
                    Pais = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IdClub = table.Column<int>(type: "integer", nullable: true),
                    Documento = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PagoAfiliacionAlDia = table.Column<bool>(type: "boolean", nullable: false),
                    Dni = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participantes", x => x.ParticipanteId);
                    table.ForeignKey(
                        name: "FK_Participantes_Categorias",
                        column: x => x.CategoriaId,
                        principalSchema: "catalogos",
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participantes_Clubes",
                        column: x => x.IdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Participantes_Sexos",
                        column: x => x.SexoId,
                        principalSchema: "catalogos",
                        principalTable: "Sexos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventoPruebas",
                schema: "regatas",
                columns: table => new
                {
                    IdEventoPrueba = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEvento = table.Column<int>(type: "integer", nullable: false),
                    IdPrueba = table.Column<int>(type: "integer", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxParticipantes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Pista = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Programada"),
                    PlanProgresionAsignado = table.Column<string>(type: "text", nullable: true),
                    PrecioCategoria = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoPruebas", x => x.IdEventoPrueba);
                    table.ForeignKey(
                        name: "FK_EventoPruebas_Eventos",
                        column: x => x.IdEvento,
                        principalSchema: "regatas",
                        principalTable: "Eventos",
                        principalColumn: "IdEvento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoPruebas_Pruebas",
                        column: x => x.IdPrueba,
                        principalSchema: "regatas",
                        principalTable: "Pruebas",
                        principalColumn: "IdPrueba",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtletasFederados",
                schema: "federacion",
                columns: table => new
                {
                    ParticipanteId = table.Column<int>(type: "integer", nullable: false),
                    IdClub = table.Column<int>(type: "integer", nullable: true),
                    IdFederacion = table.Column<int>(type: "integer", nullable: true),
                    EstadoPago = table.Column<int>(type: "integer", nullable: false),
                    PerteneceSeleccion = table.Column<bool>(type: "boolean", nullable: false),
                    Categoria = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BecadoEnard = table.Column<bool>(type: "boolean", nullable: false),
                    BecadoSdn = table.Column<bool>(type: "boolean", nullable: false),
                    MontoBeca = table.Column<decimal>(type: "numeric", nullable: false),
                    PresentoAptoMedico = table.Column<bool>(type: "boolean", nullable: false),
                    FechaAptoMedico = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtletasFederados", x => x.ParticipanteId);
                    table.ForeignKey(
                        name: "FK_AtletasFederados_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub");
                    table.ForeignKey(
                        name: "FK_AtletasFederados_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AtletasFederados_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DelegadosClub",
                schema: "federacion",
                columns: table => new
                {
                    IdParticipante = table.Column<int>(type: "integer", nullable: false),
                    IdRol = table.Column<int>(type: "integer", nullable: false),
                    IdFederacion = table.Column<int>(type: "integer", nullable: true),
                    ClubIdClub = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegadosClub", x => x.IdParticipante);
                    table.ForeignKey(
                        name: "FK_DelegadosClub_Clubes_ClubIdClub",
                        column: x => x.ClubIdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub");
                    table.ForeignKey(
                        name: "FK_DelegadosClub_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DelegadosClub_Participantes_IdParticipante",
                        column: x => x.IdParticipante,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DelegadosClub_Roles_IdRol",
                        column: x => x.IdRol,
                        principalSchema: "federacion",
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DocumentacionPersonas",
                schema: "federacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonaId = table.Column<int>(type: "integer", nullable: true),
                    TipoDocumento = table.Column<int>(type: "integer", nullable: true),
                    UrlArchivo = table.Column<string>(type: "text", nullable: false),
                    PublicId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FechaCarga = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentacionPersonas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentacionPersonas_Participantes_PersonaId",
                        column: x => x.PersonaId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId");
                });

            migrationBuilder.CreateTable(
                name: "Entrenadores",
                schema: "federacion",
                columns: table => new
                {
                    ParticipanteId = table.Column<int>(type: "integer", nullable: false),
                    IdClub = table.Column<int>(type: "integer", nullable: true),
                    IdFederacion = table.Column<int>(type: "integer", nullable: true),
                    PerteneceSeleccion = table.Column<bool>(type: "boolean", maxLength: 50, nullable: true),
                    CategoriaSeleccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BecadoEnard = table.Column<bool>(type: "boolean", nullable: true),
                    BecadoSdn = table.Column<bool>(type: "boolean", nullable: true),
                    MontoBeca = table.Column<decimal>(type: "numeric", nullable: true),
                    PresentoAptoMedico = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrenadores", x => x.ParticipanteId);
                    table.ForeignKey(
                        name: "FK_Entrenadores_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub");
                    table.ForeignKey(
                        name: "FK_Entrenadores_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Entrenadores_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagosTransacciones",
                schema: "federacion",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Concepto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Monto = table.Column<decimal>(type: "numeric", nullable: false),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FechaAprobacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IdParticipante = table.Column<int>(type: "integer", nullable: false),
                    IdClub = table.Column<int>(type: "integer", nullable: false),
                    IdMercadoPago = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosTransacciones", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_PagosTransacciones_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PagosTransacciones_Participantes_IdParticipante",
                        column: x => x.IdParticipante,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tutores",
                schema: "federacion",
                columns: table => new
                {
                    ParticipanteId = table.Column<int>(type: "integer", nullable: false),
                    TipoTutor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutores", x => x.ParticipanteId);
                    table.ForeignKey(
                        name: "FK_Tutores_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                schema: "seguridad",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RolFederacion = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IdClub = table.Column<int>(type: "integer", nullable: true),
                    IdFederacion = table.Column<int>(type: "integer", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EstaActivo = table.Column<bool>(type: "boolean", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "integer", nullable: false),
                    UltimoAcceso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Nombre = table.Column<string>(type: "text", nullable: true),
                    Apellido = table.Column<string>(type: "text", nullable: true),
                    Dni = table.Column<string>(type: "text", nullable: true),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    ParticipanteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Clubes_IdClub",
                        column: x => x.IdClub,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Usuarios_Federaciones_IdFederacion",
                        column: x => x.IdFederacion,
                        principalSchema: "federacion",
                        principalTable: "Federaciones",
                        principalColumn: "IdFederacion",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Usuarios_Participantes_ParticipanteId",
                        column: x => x.ParticipanteId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId");
                });

            migrationBuilder.CreateTable(
                name: "Etapas",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventoPruebaId = table.Column<int>(type: "integer", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Tipo = table.Column<int>(type: "integer", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etapas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etapas_EventoPruebas",
                        column: x => x.EventoPruebaId,
                        principalSchema: "regatas",
                        principalTable: "EventoPruebas",
                        principalColumn: "IdEventoPrueba",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inscripciones",
                schema: "regatas",
                columns: table => new
                {
                    IdInscripcion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdEventoPrueba = table.Column<int>(type: "integer", nullable: false),
                    IdParticipante = table.Column<int>(type: "integer", nullable: true),
                    FechaInscripcion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    NumeroCompetidor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    EsCabezaDeSerie = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Inscrito"),
                    Pagado = table.Column<bool>(type: "boolean", nullable: false),
                    AtletaFederacionParticipanteId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscripciones", x => x.IdInscripcion);
                    table.ForeignKey(
                        name: "FK_Inscripciones_AtletasFederados_AtletaFederacionParticipante~",
                        column: x => x.AtletaFederacionParticipanteId,
                        principalSchema: "federacion",
                        principalTable: "AtletasFederados",
                        principalColumn: "ParticipanteId");
                    table.ForeignKey(
                        name: "FK_Inscripciones_EventoPruebas",
                        column: x => x.IdEventoPrueba,
                        principalSchema: "regatas",
                        principalTable: "EventoPruebas",
                        principalColumn: "IdEventoPrueba",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscripciones_Participantes",
                        column: x => x.IdParticipante,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtletasTutores",
                schema: "federacion",
                columns: table => new
                {
                    IdAtleta = table.Column<int>(type: "integer", nullable: false),
                    IdTutor = table.Column<int>(type: "integer", nullable: false),
                    Parentesco = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtletasTutores", x => new { x.IdAtleta, x.IdTutor });
                    table.ForeignKey(
                        name: "FK_AtletasTutores_AtletasFederados_IdAtleta",
                        column: x => x.IdAtleta,
                        principalSchema: "federacion",
                        principalTable: "AtletasFederados",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtletasTutores_Tutores_IdTutor",
                        column: x => x.IdTutor,
                        principalSchema: "federacion",
                        principalTable: "Tutores",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fases",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EtapaId = table.Column<int>(type: "integer", nullable: false),
                    NombreFase = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumeroFase = table.Column<int>(type: "integer", nullable: false),
                    FechaHoraProgramada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FechaHoraInicioReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FechaHoraFinReal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fases_Etapas",
                        column: x => x.EtapaId,
                        principalSchema: "regatas",
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReglasProgresion",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventoPruebaId = table.Column<int>(type: "integer", nullable: false),
                    EtapaOrigenId = table.Column<int>(type: "integer", nullable: false),
                    EtapaDestinoId = table.Column<int>(type: "integer", nullable: false),
                    PosicionDesde = table.Column<int>(type: "integer", nullable: false),
                    PosicionHasta = table.Column<int>(type: "integer", nullable: false),
                    PorTiempo = table.Column<bool>(type: "boolean", nullable: false),
                    CantidadATomar = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReglasProgresion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReglasProgresion_Etapas_Destino",
                        column: x => x.EtapaDestinoId,
                        principalSchema: "regatas",
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReglasProgresion_Etapas_Origen",
                        column: x => x.EtapaOrigenId,
                        principalSchema: "regatas",
                        principalTable: "Etapas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReglasProgresion_EventoPruebas",
                        column: x => x.EventoPruebaId,
                        principalSchema: "regatas",
                        principalTable: "EventoPruebas",
                        principalColumn: "IdEventoPrueba",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InscripcionTripulantes",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InscripcionId = table.Column<int>(type: "integer", nullable: false),
                    ParticipanteId = table.Column<int>(type: "integer", nullable: false),
                    PosicionEnBote = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscripcionTripulantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscripcionTripulantes_Inscripciones",
                        column: x => x.InscripcionId,
                        principalSchema: "regatas",
                        principalTable: "Inscripciones",
                        principalColumn: "IdInscripcion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscripcionTripulantes_Participantes",
                        column: x => x.ParticipanteId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoPago = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ClubId = table.Column<int>(type: "integer", nullable: true),
                    ParticipanteId = table.Column<int>(type: "integer", nullable: true),
                    InscripcionId = table.Column<int>(type: "integer", nullable: true),
                    Monto = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    FechaPago = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    Referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RegistradoPor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Notas = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Clubes",
                        column: x => x.ClubId,
                        principalSchema: "catalogos",
                        principalTable: "Clubes",
                        principalColumn: "IdClub",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pagos_Inscripciones",
                        column: x => x.InscripcionId,
                        principalSchema: "regatas",
                        principalTable: "Inscripciones",
                        principalColumn: "IdInscripcion",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Pagos_Participantes",
                        column: x => x.ParticipanteId,
                        principalSchema: "regatas",
                        principalTable: "Participantes",
                        principalColumn: "ParticipanteId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Resultados",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FaseId = table.Column<int>(type: "integer", nullable: false),
                    InscripcionId = table.Column<int>(type: "integer", nullable: false),
                    Carril = table.Column<int>(type: "integer", nullable: true),
                    EsCabezaDeSerie = table.Column<bool>(type: "boolean", nullable: false),
                    TiempoOficial = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Posicion = table.Column<int>(type: "integer", nullable: true),
                    Puntos = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    VelocidadMedia = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Pendiente"),
                    Observaciones = table.Column<string>(type: "text", nullable: true),
                    FaseOrigenId = table.Column<int>(type: "integer", nullable: true),
                    ReglaClasificacionAplicada = table.Column<string>(type: "text", nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    FechaActualizacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UsuarioRegistro = table.Column<string>(type: "text", nullable: true),
                    UsuarioActualizacion = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resultados_Fases",
                        column: x => x.FaseId,
                        principalSchema: "regatas",
                        principalTable: "Fases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resultados_Inscripciones",
                        column: x => x.InscripcionId,
                        principalSchema: "regatas",
                        principalTable: "Inscripciones",
                        principalColumn: "IdInscripcion",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penalizaciones",
                schema: "regatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResultadoId = table.Column<int>(type: "integer", nullable: false),
                    TipoPenalizacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    TiempoPenalizacion = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Severidad = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    JuezAsignado = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalizaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalizaciones_Resultados",
                        column: x => x.ResultadoId,
                        principalSchema: "regatas",
                        principalTable: "Resultados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "catalogos",
                table: "Botes",
                columns: new[] { "Id", "Tipo" },
                values: new object[,]
                {
                    { 1, "Kayak Individual" },
                    { 2, "Kayak Doble" },
                    { 3, "Kayak Cuadruple" },
                    { 4, "Canoa Individual" },
                    { 5, "Canoa Doble" },
                    { 6, "Canoa Cuadruple" }
                });

            migrationBuilder.InsertData(
                schema: "catalogos",
                table: "Categorias",
                columns: new[] { "Id", "EdadMax", "EdadMin", "Nombre" },
                values: new object[,]
                {
                    { 1, 10, 8, "Pre-Infantil" },
                    { 2, 12, 11, "Infantil" },
                    { 3, 14, 13, "Menor" },
                    { 4, 16, 15, "Cadete" },
                    { 5, 18, 17, "Junior" },
                    { 6, 23, 19, "Sub-23" },
                    { 7, 39, 19, "Senior" },
                    { 8, 49, 40, "Master A" },
                    { 9, 59, 50, "Master B" },
                    { 10, 80, 60, "Master C" },
                    { 11, 99, 0, "Control" }
                });

            migrationBuilder.InsertData(
                schema: "catalogos",
                table: "Distancias",
                columns: new[] { "Id", "DistanciaRegata", "GapSugerido" },
                values: new object[,]
                {
                    { 1, 1, 5 },
                    { 2, 2, 5 },
                    { 3, 3, 5 },
                    { 4, 4, 5 },
                    { 5, 5, 7 },
                    { 6, 6, 10 },
                    { 7, 7, 10 },
                    { 8, 8, 10 },
                    { 9, 9, 15 },
                    { 10, 10, 15 },
                    { 11, 11, 20 },
                    { 12, 12, 20 },
                    { 13, 13, 25 },
                    { 14, 14, 25 },
                    { 15, 15, 30 },
                    { 16, 16, 40 }
                });

            migrationBuilder.InsertData(
                schema: "catalogos",
                table: "PlanesSaaS",
                columns: new[] { "Id", "ExportacionExcel", "MaxAtletas", "MaxTorneosActivos", "Nombre", "Precio", "ResultadosTiempoReal", "SoportePrioritario" },
                values: new object[,]
                {
                    { 1, false, 500, 5, "SIGDEF (S)", 50m, false, false },
                    { 2, false, 2000, 20, "SIGDEF (M)", 120m, false, false },
                    { 3, true, -1, -1, "SIGDEF (L)", 250m, true, true },
                    { 4, false, 500, 5, "SportTrack (S)", 40m, false, false },
                    { 5, false, 2000, 20, "SportTrack (M)", 90m, false, false },
                    { 6, true, -1, -1, "SportTrack (L)", 190m, true, true },
                    { 7, true, 500, 5, "Pack DÃºo (S)", 75m, true, true },
                    { 8, true, 2000, 20, "Pack DÃºo (M)", 170m, true, true },
                    { 9, true, -1, -1, "Pack DÃºo (L)", 350m, true, true }
                });

            migrationBuilder.InsertData(
                schema: "catalogos",
                table: "Sexos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Masculino" },
                    { 2, "Femenino" },
                    { 3, "Mixto" }
                });

            migrationBuilder.InsertData(
                schema: "seguridad",
                table: "Usuarios",
                columns: new[] { "IdUsuario", "Apellido", "Dni", "Email", "EstaActivo", "FechaCreacion", "IdClub", "IdFederacion", "IntentosFallidos", "Nombre", "ParticipanteId", "PasswordHash", "RolFederacion", "Telefono", "UltimoAcceso", "Username" },
                values: new object[] { 1, null, null, "admin@sporttrack.com", true, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, 0, null, null, "$2a$12$R9h/lSAbvI125hcnyqvQDu9fAKDLn6Y8yK/.Vz0uI3492M0h0mY3.", "SuperAdmin", null, null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_AtletasFederados_IdClub",
                schema: "federacion",
                table: "AtletasFederados",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_AtletasFederados_IdFederacion",
                schema: "federacion",
                table: "AtletasFederados",
                column: "IdFederacion");

            migrationBuilder.CreateIndex(
                name: "IX_AtletasTutores_IdTutor",
                schema: "federacion",
                table: "AtletasTutores",
                column: "IdTutor");

            migrationBuilder.CreateIndex(
                name: "IX_Botes_Tipo",
                schema: "catalogos",
                table: "Botes",
                column: "Tipo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                schema: "catalogos",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubes_IdFederacion",
                schema: "catalogos",
                table: "Clubes",
                column: "IdFederacion");

            migrationBuilder.CreateIndex(
                name: "IX_Clubes_Nombre",
                schema: "catalogos",
                table: "Clubes",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clubes_PlanSaaSId",
                schema: "catalogos",
                table: "Clubes",
                column: "PlanSaaSId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegadosClub_ClubIdClub",
                schema: "federacion",
                table: "DelegadosClub",
                column: "ClubIdClub");

            migrationBuilder.CreateIndex(
                name: "IX_DelegadosClub_IdFederacion",
                schema: "federacion",
                table: "DelegadosClub",
                column: "IdFederacion");

            migrationBuilder.CreateIndex(
                name: "IX_DelegadosClub_IdRol",
                schema: "federacion",
                table: "DelegadosClub",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "IX_Distancias_DistanciaRegata",
                schema: "catalogos",
                table: "Distancias",
                column: "DistanciaRegata",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentacionPersonas_PersonaId",
                schema: "federacion",
                table: "DocumentacionPersonas",
                column: "PersonaId");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenadores_IdClub",
                schema: "federacion",
                table: "Entrenadores",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_Entrenadores_IdFederacion",
                schema: "federacion",
                table: "Entrenadores",
                column: "IdFederacion");

            migrationBuilder.CreateIndex(
                name: "IX_Etapas_EventoPruebaId",
                schema: "regatas",
                table: "Etapas",
                column: "EventoPruebaId");

            migrationBuilder.CreateIndex(
                name: "IX_EventoPruebas_Estado",
                schema: "regatas",
                table: "EventoPruebas",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_EventoPruebas_EventoPrueba_Fecha",
                schema: "regatas",
                table: "EventoPruebas",
                columns: new[] { "IdEvento", "IdPrueba", "FechaHora" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventoPruebas_FechaHora",
                schema: "regatas",
                table: "EventoPruebas",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_EventoPruebas_IdPrueba",
                schema: "regatas",
                table: "EventoPruebas",
                column: "IdPrueba");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Estado",
                schema: "regatas",
                table: "Eventos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Fecha",
                schema: "regatas",
                table: "Eventos",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_IdClub",
                schema: "regatas",
                table: "Eventos",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_IdFederacion",
                schema: "regatas",
                table: "Eventos",
                column: "IdFederacion");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_Nombre",
                schema: "regatas",
                table: "Eventos",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Fases_EtapaId",
                schema: "regatas",
                table: "Fases",
                column: "EtapaId");

            migrationBuilder.CreateIndex(
                name: "IX_Federaciones_PlanSaaSId",
                schema: "federacion",
                table: "Federaciones",
                column: "PlanSaaSId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_AtletaFederacionParticipanteId",
                schema: "regatas",
                table: "Inscripciones",
                column: "AtletaFederacionParticipanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_Estado",
                schema: "regatas",
                table: "Inscripciones",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_FechaInscripcion",
                schema: "regatas",
                table: "Inscripciones",
                column: "FechaInscripcion");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_IdEventoPrueba",
                schema: "regatas",
                table: "Inscripciones",
                column: "IdEventoPrueba");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_NumeroCompetidor",
                schema: "regatas",
                table: "Inscripciones",
                column: "NumeroCompetidor");

            migrationBuilder.CreateIndex(
                name: "IX_Inscripciones_ParticipanteId",
                schema: "regatas",
                table: "Inscripciones",
                column: "IdParticipante");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionTripulantes_ParticipanteId",
                schema: "regatas",
                table: "InscripcionTripulantes",
                column: "ParticipanteId");

            migrationBuilder.CreateIndex(
                name: "IX_InscripcionTripulantes_Unica",
                schema: "regatas",
                table: "InscripcionTripulantes",
                columns: new[] { "InscripcionId", "ParticipanteId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ClubId",
                schema: "regatas",
                table: "Pagos",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_InscripcionId",
                schema: "regatas",
                table: "Pagos",
                column: "InscripcionId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ParticipanteId",
                schema: "regatas",
                table: "Pagos",
                column: "ParticipanteId");

            migrationBuilder.CreateIndex(
                name: "IX_PagosTransacciones_IdClub",
                schema: "federacion",
                table: "PagosTransacciones",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_PagosTransacciones_IdParticipante",
                schema: "federacion",
                table: "PagosTransacciones",
                column: "IdParticipante");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_CategoriaId",
                schema: "regatas",
                table: "Participantes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_ClubId",
                schema: "regatas",
                table: "Participantes",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_Email",
                schema: "regatas",
                table: "Participantes",
                column: "Email",
                unique: true,
                filter: "\"Email\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_NombreApellido",
                schema: "regatas",
                table: "Participantes",
                columns: new[] { "Nombre", "Apellido" });

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_Pais",
                schema: "regatas",
                table: "Participantes",
                column: "Pais");

            migrationBuilder.CreateIndex(
                name: "IX_Participantes_SexoId",
                schema: "regatas",
                table: "Participantes",
                column: "SexoId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalizaciones_JuezAsignado",
                schema: "regatas",
                table: "Penalizaciones",
                column: "JuezAsignado");

            migrationBuilder.CreateIndex(
                name: "IX_Penalizaciones_ResultadoId",
                schema: "regatas",
                table: "Penalizaciones",
                column: "ResultadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalizaciones_ResultadoTipo",
                schema: "regatas",
                table: "Penalizaciones",
                columns: new[] { "ResultadoId", "TipoPenalizacion" });

            migrationBuilder.CreateIndex(
                name: "IX_Penalizaciones_Severidad",
                schema: "regatas",
                table: "Penalizaciones",
                column: "Severidad");

            migrationBuilder.CreateIndex(
                name: "IX_Penalizaciones_TipoPenalizacion",
                schema: "regatas",
                table: "Penalizaciones",
                column: "TipoPenalizacion");

            migrationBuilder.CreateIndex(
                name: "IX_PlanesSaaS_Nombre",
                schema: "catalogos",
                table: "PlanesSaaS",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_CategoriaEdad",
                schema: "regatas",
                table: "Pruebas",
                column: "CategoriaEdad");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_DistanciaId",
                schema: "regatas",
                table: "Pruebas",
                column: "DistanciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_Nombre",
                schema: "regatas",
                table: "Pruebas",
                column: "Nombre");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_SexoCompetencia",
                schema: "regatas",
                table: "Pruebas",
                column: "SexoCompetencia");

            migrationBuilder.CreateIndex(
                name: "IX_Pruebas_Unica",
                schema: "regatas",
                table: "Pruebas",
                columns: new[] { "TipoBote", "CategoriaEdad", "DistanciaId", "SexoCompetencia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReglasProgresion_EtapaDestinoId",
                schema: "regatas",
                table: "ReglasProgresion",
                column: "EtapaDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasProgresion_EtapaOrigenId",
                schema: "regatas",
                table: "ReglasProgresion",
                column: "EtapaOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_ReglasProgresion_EventoPruebaId",
                schema: "regatas",
                table: "ReglasProgresion",
                column: "EventoPruebaId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_Carril",
                schema: "regatas",
                table: "Resultados",
                columns: new[] { "FaseId", "Carril" });

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_InscripcionId",
                schema: "regatas",
                table: "Resultados",
                column: "InscripcionId");

            migrationBuilder.CreateIndex(
                name: "IX_Sexos_Nombre",
                schema: "catalogos",
                table: "Sexos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                schema: "seguridad",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdClub",
                schema: "seguridad",
                table: "Usuarios",
                column: "IdClub");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdFederacion",
                schema: "seguridad",
                table: "Usuarios",
                column: "IdFederacion");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ParticipanteId",
                schema: "seguridad",
                table: "Usuarios",
                column: "ParticipanteId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Username",
                schema: "seguridad",
                table: "Usuarios",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtletasTutores",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "DelegadosClub",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "DocumentacionPersonas",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "Entrenadores",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "InscripcionTripulantes",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Pagos",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "PagosTransacciones",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "Penalizaciones",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "ReglasProgresion",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Usuarios",
                schema: "seguridad");

            migrationBuilder.DropTable(
                name: "Tutores",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "Resultados",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Fases",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Inscripciones",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Etapas",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "AtletasFederados",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "EventoPruebas",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Participantes",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Eventos",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Pruebas",
                schema: "regatas");

            migrationBuilder.DropTable(
                name: "Clubes",
                schema: "catalogos");

            migrationBuilder.DropTable(
                name: "Botes",
                schema: "catalogos");

            migrationBuilder.DropTable(
                name: "Categorias",
                schema: "catalogos");

            migrationBuilder.DropTable(
                name: "Distancias",
                schema: "catalogos");

            migrationBuilder.DropTable(
                name: "Sexos",
                schema: "catalogos");

            migrationBuilder.DropTable(
                name: "Federaciones",
                schema: "federacion");

            migrationBuilder.DropTable(
                name: "PlanesSaaS",
                schema: "catalogos");
        }
    }
}
