# SportTrack-Sigdef - Backend Context & Migration Document

Este documento sirve como resumen y contexto de todo el trabajo de unificación y migración realizado para consolidar los backends de `SportTrack` y `SportTrack-Federaciones` en una única solución robusta: `SportTrack-Sigdef`.

## Objetivo Principal
El objetivo fundamental fue unificar las bases de código para evitar duplicidad de clases, entidades, y servicios. Ahora, el sistema soporta una arquitectura multi-inquilino (multi-federación), donde un SuperAdmin puede administrar múltiples federaciones, y cada federación cuenta con su propio ecosistema de clubes y atletas de forma aislada.

## Estructura de la Solución

La solución `SportTrack-Sigdef` se compone principalmente de dos proyectos tras la migración:

### 1. `SportTrack-Sigdef.Entidades`
Contiene todos los modelos de dominio, entidades y enumeraciones (Enums) del sistema.
- **Enums:** Se unificaron las enumeraciones de ambos proyectos anteriores en la carpeta `Enums` para uso global.
- **Entidades Globales:** Se consolidaron clases que comparten la misma estructura lógica, tales como `Usuario`, `Atleta`, `Club`, `Evento`, `SaaS`, y `Pago`. Las propiedades clave como `Id` (primarias) y claves foráneas como `IdClub`, `IdFederacion`, etc., fueron estandarizadas.
- **Entidades de Federación:** Entidades que tenían particularidades estrictamente relacionadas a la lógica de la Federación ahora forman parte del mismo ensamblado, pero se integran de manera fluida (ej. se añadieron relaciones necesarias para separar árboles genealógicos).

### 2. `SportTrack-Sigdef.Controladores`
Contiene la capa de lógica de negocio (Servicios), acceso a datos (Repositorios), presentación de APIs (Controladores), y clases de apoyo (DTOs, Hubs, Extensions, Exceptions).

#### Consolidación Realizada:
- **Interfaces y Servicios:** Se migraron y unificaron servicios y sus interfaces (`IAtletaService`, `IEventoService`, `IPagoService`, etc.). Se resolvieron conflictos en propiedades duplicadas y errores de referencias (ej. uso consistente de `Id` en vez de `IdAtleta` o similar cuando la clase heredaba o se unificaba).
- **Controladores:** Se integraron todos los controladores, ajustando la inyección de dependencias y referenciando a las interfaces unificadas.
- **DTOs y MappingProfile:** Se consolidaron los objetos de transferencia de datos y sus mapeos (AutoMapper) para garantizar que las respuestas de la API sean consistentes.
- **Resolución de Conflictos Técnicos:** 
  - Se corrigieron extensiones y métodos auxiliares (`DistanciaRegataExtensions`, `CategoriaEdadExtensions`) que presentaban ambigüedades.
  - Se resolvieron las referencias LINQ en los repositorios e interfaces para alinearse con los nuevos nombres de propiedades de las entidades base.
  - Se solventaron consultas proyectadas complejas (proyecciones en memoria en `EventoService` y `EventoPruebaController`) para mapeos directos a DTOs.

## Estado Actual de la Base de Código
- **Compilación Limpia:** El backend ha pasado por un proceso intensivo de corrección de errores de sintaxis y referencias, logrando una **compilación exitosa (0 errores)**.
- **Arquitectura Lista:** La base de datos y su contexto están preparados para admitir flujos de usuarios SuperAdmin, administradores de Federación, delegados de Clubes y Atletas desde un único punto de verdad, previniendo el cruce de datos mediante la relación mandatoria con `Federacion`.

---

*Nota para el Asistente IA: Utiliza este archivo como punto de referencia para entender el contexto arquitectónico y los nombres de las propiedades/clases a la hora de implementar nuevas funcionalidades o realizar consultas sobre el backend unificado.*
