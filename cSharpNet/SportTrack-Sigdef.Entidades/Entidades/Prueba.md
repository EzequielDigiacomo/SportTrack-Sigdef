# Prueba.cs

## Qué es este archivo

Plantilla de una **prueba deportiva**: combinación lógica de tipo de bote, categoría de edad, distancia y sexo de competencia, con nombre y descripción. Se instancia muchas veces vía `EventoPrueba`.

## Conceptos C# que aparecen

| Concepto | Explicación |
|----------|-------------|
| FKs como `int` | `TipoBote`, `CategoriaEdad`, etc. (nombres confusos: parecen enums pero son ids). |
| Navegaciones tipadas | `Bote`, `Categoria`, `Distancia`, `Sexo`. |
| Colección | `EventoPruebas`. |

## Namespace y usings

- **Namespace:** `SportTrack_Sigdef.Entidades.Entidades`

## Miembros

| Propiedad | Tipo | Significado |
|-----------|------|-------------|
| `IdPrueba` | `int` | PK. |
| `Nombre` | `string` | Ej. "K1 Senior M 500m". |
| `TipoBote` | `int` | FK hacia `Bote` (nombre sugiere enum). |
| `CategoriaEdad` | `int` | FK hacia `Categoria`. |
| `DistanciaId` | `int` | FK `Distancia`. |
| `SexoCompetencia` | `int` | FK `Sexo`. |
| `Descripcion` | `string?` | Extra. |
| `Bote` / `Categoria` / `Distancia` / `Sexo` | entidades | Navegaciones. |
| `EventoPruebas` | colección | Usos en eventos. |

## Relaciones

N→1 catálogos; 1→N `EventoPrueba`.

## Notas de estudio

1. Los nombres `TipoBote` / `SexoCompetencia` como `int` chocan con enums homónimos: al leer código, confirmá el mapeo en `DbContext`.
2. La prueba es **inmutable conceptualmente**; lo que cambia por evento es `EventoPrueba`.
