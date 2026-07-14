using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
using SportTrack_Sigdef.AccesoDatos;
using SportTrack_Sigdef.Controladores.Audit;
using SportTrack_Sigdef.Controladores.Auth;
using SportTrack_Sigdef.Controladores.Bote;
using SportTrack_Sigdef.Controladores.Categoria;
using SportTrack_Sigdef.Controladores.Club;
using SportTrack_Sigdef.Controladores.Distancia;
using SportTrack_Sigdef.Controladores.Evento;
using SportTrack_Sigdef.Controladores.Fase;
using SportTrack_Sigdef.Controladores.Hubs;
using SportTrack_Sigdef.Controladores.Inscripcion;
using SportTrack_Sigdef.Controladores.Mappings;
using SportTrack_Sigdef.Controladores.Pago;
using SportTrack_Sigdef.Controladores.Participante;
using SportTrack_Sigdef.Controladores.Resultado;
using SportTrack_Sigdef.Controladores.SaaS;
using SportTrack_Sigdef.Controladores.Federaciones;
using SportTrack_Sigdef.Controladores.Mensajes;
using SportTrack_Sigdef.Controladores.Services;
using SportTrack_Sigdef.Controladores.Documentacion;
using SportTrack_Sigdef.Controladores.PagosSIGDEF.Extensions;
using SportTrack_Sigdef.Controladores.PagosSIGDEF.Services;
using SIGDEF.API.Services;
using SportTrack_Sigdef.Middleware;
using SportTrack_Sigdef;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos PostgreSQL
var connectionString = ResolveConnectionString(builder.Configuration);
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("ADVERTENCIA: No hay connection string (DefaultConnection ni DATABASE_URL).");
}
else
{
    Console.WriteLine("Connection string configurada (origen: " +
        (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("DefaultConnection")) ? "DATABASE_URL" : "DefaultConnection") + ").");
}

builder.Services.AddDbContext<SportTrackDbContext>(options =>
    options.UseNpgsql(connectionString));

// SignalR para tiempo real
builder.Services.AddSignalR();

// Configuración de CORS (lista blanca; no AllowAnyOrigin)
var originsConfig = builder.Configuration["AllowedOrigins"];
var configOrigins = !string.IsNullOrEmpty(originsConfig)
    ? originsConfig.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(o => o.Trim()).ToArray()
    : Array.Empty<string>();
var allowedOrigins = configOrigins.Concat(new[]
{
    "http://localhost:3000",
    "http://localhost:5173",
    "https://sporttrack-fec.vercel.app",
    "https://sigdef.vercel.app",
    "https://oficialsporttrack.vercel.app"
}).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

var corsAllowedOrigins = new CorsAllowedOrigins(allowedOrigins);
builder.Services.AddSingleton(corsAllowedOrigins);

Console.WriteLine($"Configurando CORS para orígenes: {string.Join(", ", allowedOrigins)}");

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .WithOrigins(allowedOrigins)
              .AllowCredentials();
    });
});

// Autenticación JWT — TokenKey obligatorio fuera de Development
var tokenKey = SportTrack_Sigdef.Security.TokenKeyResolver.Resolve(builder.Configuration, builder.Environment);

// Fase 2: autenticado por defecto. Solo [AllowAnonymous] explícito queda público (Live, login, health, webhook…).
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Fase 3: rate limit en login/register (por IP)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString()
                          ?? httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                          ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            NameClaimType = System.Security.Claims.ClaimTypes.Name
        };

        // Soporte para SignalR con JWT en el query string o Cookies
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Preferir Authorization header (cross-origin: cookie third-party suele bloquearse)
                var authHeader = context.Request.Headers.Authorization.ToString();
                if (!string.IsNullOrEmpty(authHeader) &&
                    authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Token = authHeader["Bearer ".Length..].Trim();
                    return Task.CompletedTask;
                }

                var accessToken = context.Request.Query["access_token"];
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = context.Request.Cookies["X-Access-Token"];
                }

                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Inyección de Dependencias (Core)
// Botes
builder.Services.AddScoped<IBoteService, BoteService>();
builder.Services.AddScoped<IBoteRepository, BoteRepository>();
// Categorias
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
// Distancias
builder.Services.AddScoped<IDistanciaService, DistanciaService>();
builder.Services.AddScoped<IDistanciaRepository, DistanciaRepository>();
// Inscripciones
builder.Services.AddScoped<IInscripcionService, InscripcionService>();
builder.Services.AddScoped<IInscripcionRepository, InscripcionRepository>();
// Participantes
builder.Services.AddScoped<IParticipanteService, ParticipanteService>();
builder.Services.AddScoped<IParticipanteRepository, ParticipanteRepository>();
// Eventos
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
// Fases y Resultados
builder.Services.AddScoped<IEtapaRepository, EtapaRepository>();
builder.Services.AddScoped<IFaseRepository, FaseRepository>();
builder.Services.AddScoped<IFaseService, FaseService>();
builder.Services.AddScoped<IResultadoRepository, ResultadoRepository>();
// Clubes
builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
// Auth
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
// SaaS
builder.Services.AddScoped<ISaaSService, SaaSService>();
// Auditoria
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuditService, AuditService>();
// Mensajería privada
builder.Services.AddScoped<IMensajeRepository, MensajeRepository>();
builder.Services.AddScoped<IMensajeService, MensajeService>();
// Pagos
builder.Services.AddScoped<IPagoService, PagoService>();

// Inyección de Dependencias (SIGDEF / Federaciones / Multi-tenant)
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<IAltaAtletaService, AltaAtletaService>();
builder.Services.AddScoped<IAtletaServices, AtletaServices>();
builder.Services.AddScoped<IAtletaTutorServices, AtletaTutorServices>();
builder.Services.AddScoped<IClubServices, ClubServices>();
builder.Services.AddScoped<IDelegadoClubServices, DelegadoClubServices>();
builder.Services.AddScoped<IEntrenadorServices, EntrenadorServices>();
builder.Services.AddScoped<IEventoServices, EventoServices>();
builder.Services.AddScoped<IFederacionServices, FederacionServices>();
builder.Services.AddScoped<IInscripcionServices, InscripcionServices>();
builder.Services.AddScoped<IPersonaServices, PersonaServices>();
builder.Services.AddScoped<IPruebaServices, PruebaServices>();
builder.Services.AddScoped<IRolServices, RolServices>();
builder.Services.AddScoped<ITutorServices, TutorServices>();
builder.Services.AddScoped<IUsuarioServices, UsuarioServices>();

// Documentación (Cloudinary + fallback local)
builder.Services.Configure<CloudinarySettings>(options =>
{
    var section = builder.Configuration.GetSection("CloudinarySettings");
    options.CloudName = section["CloudName"]
        ?? builder.Configuration["CLOUDINARY_CLOUD_NAME"]
        ?? Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME")
        ?? string.Empty;
    options.ApiKey = section["ApiKey"]
        ?? builder.Configuration["CLOUDINARY_API_KEY"]
        ?? Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY")
        ?? string.Empty;
    options.ApiSecret = section["ApiSecret"]
        ?? builder.Configuration["CLOUDINARY_API_SECRET"]
        ?? Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET")
        ?? string.Empty;
});
builder.Services.AddScoped<IDocumentacionService, DocumentacionService>();

// MercadoPago Services
builder.Services.AddMercadoPagoServices();

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Registrar Controladores incluyendo el ensamblado de Controladores referenciado
builder.Services.AddControllers()
    .AddApplicationPart(typeof(SportTrack_Sigdef.Controladores.Controllers.PagoTransaccionController).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SportTrack SIGDEF API", Version = "v1" });

    // Configurar el botón 'Authorize' para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configuración para leer IP/esquema real a través del proxy de Render
var forwardedOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedOptions.KnownNetworks.Clear();
forwardedOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedOptions);

// Ejecutar migraciones automáticamente al iniciar (útil para Render)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SportTrackDbContext>();
        if (!await context.Database.CanConnectAsync())
        {
            Console.WriteLine("ERROR: No se pudo conectar a PostgreSQL. Revisá ConnectionStrings__DefaultConnection o DATABASE_URL.");
        }
        else
        {
            var pending = (await context.Database.GetPendingMigrationsAsync()).ToList();
            if (pending.Count > 0)
            {
                Console.WriteLine($"Aplicando {pending.Count} migración(es) pendiente(s)...");
                await context.Database.MigrateAsync();
                Console.WriteLine("Migraciones aplicadas con éxito.");
            }
            else
            {
                Console.WriteLine("Base de datos al día (sin migraciones pendientes).");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR al aplicar migraciones: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"  Inner: {ex.InnerException.Message}");
    }
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Hub: negotiate/connect anónimo (Live); Join*/GetServerTime AllowAnonymous; escrituras Authorize por método
app.MapHub<TimingHub>("/hubs/timing").AllowAnonymous();

app.Run();

static string? ResolveConnectionString(IConfiguration configuration)
{
    var fromConfig = configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrWhiteSpace(fromConfig))
        return fromConfig;

    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (string.IsNullOrWhiteSpace(databaseUrl))
        return null;

    // Render puede entregar postgres:// — Npgsql 8 acepta postgresql://
    if (databaseUrl.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase))
        databaseUrl = "postgresql://" + databaseUrl["postgres://".Length..];

    return databaseUrl;
}
