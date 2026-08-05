# MappingProfile

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Mappings/MappingProfile.cs`

## 1. Qué es este archivo

Es un **Perfil de AutoMapper (mapeos entidad ↔ DTO)** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Hereda/implementa: `Profile`.

## 2. Conceptos C# / .NET que aparecen

- **AutoMapper**: biblioteca que mapea propiedades entre entidades y DTOs (`CreateMap`, `_mapper.Map`).

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Mappings`
- **Usings:**
  - `using AutoMapper;`
  - `using SportTrack_Sigdef.Controladores.Bote.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Categoria.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Distancia.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Inscripcion.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Evento.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Auth.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Club.Dtos;`
  - `using SportTrack_Sigdef.Controladores.Participante.Dtos;`
  - `using SportTrack_Sigdef.Entidades.Entidades;`
  - `using SportTrack_Sigdef.Entidades.Enums;`

## 4. Detalle del tipo — tipo principal

### Constructores

#### Constructor 1: `MappingProfile(...)`

**Parámetros:**

_sin parámetros_

**Qué hace:** configura los mapeos AutoMapper (`CreateMap`) entre entidades y DTOs en el constructor del `Profile`.

**Mapeos detectados (muestra):**

- `CreateMap<Entidades.Entidades.Club, ClubDto>`
- `CreateMap<ClubCreateDto, Entidades.Entidades.Club>`
- `CreateMap<ClubUpdateDto, Entidades.Entidades.Club>`
- `CreateMap<Entidades.Entidades.Bote, BoteDto>`
- `CreateMap<BoteCreateDto, Entidades.Entidades.Bote>`
- `CreateMap<BoteUpdateDto, Entidades.Entidades.Bote>`
- `CreateMap<Entidades.Entidades.Categoria, CategoriaDto>`
- `CreateMap<CategoriaCreateDto, Entidades.Entidades.Categoria>`
- `CreateMap<CategoriaUpdateDto, Entidades.Entidades.Categoria>`
- `CreateMap<Entidades.Entidades.Distancia, DistanciaDto>`
- `CreateMap<DistanciaCreateDto, Entidades.Entidades.Distancia>`
- `CreateMap<DistanciaUpdateDto, Entidades.Entidades.Distancia>`
- `CreateMap<InscripcionTripulante, InscripcionTripulanteDto>`
- `CreateMap<InscripcionTripulanteCreateDto, InscripcionTripulante>`
- `CreateMap<Entidades.Entidades.Inscripcion, InscripcionDto>`
- `CreateMap<InscripcionCreateDto, Entidades.Entidades.Inscripcion>`
- `CreateMap<InscripcionUpdateDto, Entidades.Entidades.Inscripcion>`
- `CreateMap<Entidades.Entidades.Evento, EventoDto>`
- `CreateMap<EventoCreateDto, Entidades.Entidades.Evento>`
- `CreateMap<EventoUpdateDto, Entidades.Entidades.Evento>`
- `CreateMap<Entidades.Entidades.Fase, SportTrack_Sigdef.Controladores.Fase.Dtos.FaseDto>`
- `CreateMap<Entidades.Entidades.Resultado, SportTrack_Sigdef.Controladores.Fase.Dtos.ResultadoFaseDto>`
- `CreateMap<Usuario, AuthResponseDto>`
- `CreateMap<RegisterDto, Usuario>`
- `CreateMap<Usuario, UsuarioDto>`
- `CreateMap<Entidades.Entidades.Participante, ParticipanteDto>`
- `CreateMap<ParticipanteCreateDto, Entidades.Entidades.Participante>`
- `CreateMap<Sexo, SexoDto>`
- `CreateMap<Prueba, PruebaDto>`
- `CreateMap<EventoPrueba, EventoPruebaDto>`
- `CreateMap<PlanSaaS, SportTrack_Sigdef.Controladores.SaaS.Dtos.PlanSaaSDto>`

## 5. Notas de estudio

- Revisá `Mappings/MappingProfile.cs` para ver cómo se mapea esta entidad/DTO.
- Ruta relativa en el proyecto: `Mappings/MappingProfile.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
