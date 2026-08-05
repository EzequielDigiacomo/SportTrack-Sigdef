# cSharpNet — Guía de estudio C# / .NET con SportTrack-Sigdef

Material de aprendizaje basado en el código real de este repositorio.
Cada carpeta de proyecto aquí espeja un proyecto `.csproj` del solution.

## Cómo está organizado el solution

```
SportTrack-Sigdef.sln
├── SportTrack-Sigdef              → API ASP.NET Core (Controllers, Program.cs, Middleware)
├── SportTrack-Sigdef.Controladores → Lógica de negocio (Services, Repositories, DTOs)
├── SportTrack-Sigdef.Entidades     → Modelo de dominio (entidades, enums)
└── SportTrack-Sigdef.AccesoDatos   → EF Core DbContext (acceso a base de datos)
```

Flujo típico de una petición HTTP:

```
Cliente → Controller (API) → Service → Repository / DbContext → SQL Server
                ↓
              DTO (entrada/salida)
                ↓
            Entidad (persistencia)
```

## Orden de estudio recomendado

1. **Fundamentos/** — conceptos generales de C# y .NET
2. **SportTrack-Sigdef.Entidades** — clases, propiedades, enums, atributos
3. **SportTrack-Sigdef.AccesoDatos** — DbContext y Entity Framework Core
4. **SportTrack-Sigdef.Controladores** — interfaces, async, servicios, DTOs
5. **SportTrack-Sigdef** — Controllers, middleware, Program.cs, autenticación

## Qué encontrarás en cada `.md`

Para cada archivo `.cs` del código fuente hay un `.md` homónimo que detalla:

- Qué hace el archivo
- Conceptos de C# / .NET que aparecen
- Namespace y `using`
- Cada atributo, propiedad y método
- Notas de estudio

## Carpetas de este directorio

| Carpeta | Contenido |
|---------|-----------|
| `Fundamentos/` | Lecciones transversales (clases, API, EF Core, async, DTOs) |
| `SportTrack-Sigdef.Entidades/` | Docs de entidades y enums |
| `SportTrack-Sigdef.AccesoDatos/` | Docs del DbContext |
| `SportTrack-Sigdef.Controladores/` | Docs de servicios, repos y DTOs |
| `SportTrack-Sigdef/` | Docs de la API web |

## Tips para leer el código

1. Abre el `.cs` y el `.md` lado a lado.
2. Busca primero el **tipo** (`class`, `interface`, `enum`, `record`).
3. Luego las **propiedades** (`get; set;`) y los **métodos**.
4. Fíjate en los **atributos** entre corchetes: `[HttpGet]`, `[Required]`, `[Key]`.
5. Si ves `async` / `await` / `Task`, es programación asíncrona.
6. Si ves `ISomething` inyectado en un constructor, es inyección de dependencias (DI).

## Glosario rápido

| Término | Significado |
|---------|-------------|
| **Namespace** | Espacio de nombres; organiza tipos (como un paquete) |
| **using** | Importa un namespace para no escribir el nombre completo |
| **class** | Tipo referencia; plantilla de objetos |
| **interface** | Contrato (métodos/propiedades) que una clase implementa |
| **enum** | Conjunto fijo de valores con nombre |
| **DTO** | Data Transfer Object: objeto para enviar/recibir datos por la API |
| **Entity** | Clase que mapea a una tabla de base de datos |
| **DbContext** | Puerta de entrada de EF Core a la base de datos |
| **async/await** | Permite operaciones I/O sin bloquear el hilo |
| **DI** | Inyección de dependencias: el framework te pasa las dependencias |
| **Middleware** | Pieza del pipeline HTTP (errores, seguridad, CORS, etc.) |
| **Controller** | Expone endpoints HTTP (GET, POST, PUT, DELETE) |
