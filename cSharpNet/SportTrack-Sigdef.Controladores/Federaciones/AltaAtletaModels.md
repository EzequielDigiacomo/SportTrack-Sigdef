# AltaAtletaParticipanteInput

**Archivo fuente:** `SportTrack-Sigdef.Controladores/Federaciones/AltaAtletaModels.cs`

## 1. Qué es este archivo

Es un **Modelo auxiliar de la capa de negocio** de la capa `SportTrack-Sigdef.Controladores` (servicios, repositorios y DTOs).

Este archivo declara varios tipos:
- `class AltaAtletaParticipanteInput`
- `class AltaAtletaFederacionInput`
- `class AltaAtletaResult`

## 2. Conceptos C# / .NET que aparecen

- **Tipos C#**: clases/interfaces organizadas por namespace en la capa de lógica de negocio.

## 3. Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Controladores.Federaciones`
- **Usings:**
  - `using SportTrack_Sigdef.Entidades.Enums;`
  - `using System;`

## 4. Detalle del tipo — `class AltaAtletaParticipanteInput`

### Propiedades

#### `Nombre`

- **Tipo:** `string`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Apellido`

- **Tipo:** `string`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Documento`

- **Tipo:** `string`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `FechaNacimiento`

- **Tipo:** `DateTime`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `SexoId`

- **Tipo:** `int`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `IdClub`

- **Tipo:** `int?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Email`

- **Tipo:** `string?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Telefono`

- **Tipo:** `string?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Direccion`

- **Tipo:** `string?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `CategoriaId`

- **Tipo:** `int?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Pais`

- **Tipo:** `string?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `PagoAfiliacionAlDia`

- **Tipo:** `bool`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración


## 4. Detalle del tipo — `class AltaAtletaFederacionInput`

### Propiedades

#### `IdClub`

- **Tipo:** `int?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `IdFederacion`

- **Tipo:** `int?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `EstadoPago`

- **Tipo:** `EstadoPago`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `PerteneceSeleccion`

- **Tipo:** `bool`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Categoria`

- **Tipo:** `CategoriaEdad?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `BecadoEnard`

- **Tipo:** `bool`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `BecadoSdn`

- **Tipo:** `bool`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `MontoBeca`

- **Tipo:** `decimal`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `PresentoAptoMedico`

- **Tipo:** `bool`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `FechaAptoMedico`

- **Tipo:** `DateTime?`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración


## 4. Detalle del tipo — `class AltaAtletaResult`

### Propiedades

#### `ParticipanteId`

- **Tipo:** `int`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `Participante`

- **Tipo:** `Entidades.Entidades.Participante`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `AtletaFederacion`

- **Tipo:** `Entidades.Entidades.AtletaFederacion`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `ParticipanteCreado`

- **Tipo:** `bool`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración

#### `AtletaFederacionCreado`

- **Tipo:** `bool`
- **Acceso:** propiedad con `get`/`set` (o init) según declaración


## 5. Notas de estudio

- La carpeta `Federaciones` concentra muchos servicios multi-tenant (federación/club) y DTOs por entidad de dominio.
- Ruta relativa en el proyecto: `Federaciones/AltaAtletaModels.cs`.

---

*Documentación generada para aprendizaje C#/.NET a partir del código fuente real.*
