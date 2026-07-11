# Guía — Atletas, club y tutores

## Atletas por club

`ClubServices` / endpoint de atletas del club debe proyectar al menos:

- `ParticipanteId`
- Datos de persona
- **`FechaNacimiento`**
- `Documento` (si se usa en UI)

Sin fecha, el front no puede calcular edad → columna Tutor queda en “—” o usa fallbacks por categoría.

### Cambio guardado (2026-07)

Se agregaron `FechaNacimiento` / documento al `Select` de atletas por club para que FrontSigdef muestre ✅/❌ correctamente.

**Nota:** hace falta **redeploy** de la API en el entorno (ej. Render) para que producción lo tenga.

## AtletaTutor

| Campo | Significado |
|-------|-------------|
| `ParticipanteId` | Atleta |
| `IdTutor` | Tutor |

El front debe POST/DELETE con esos nombres (no `idAtleta`).

## Tutor

- Entidad tutor ligada a `ParticipanteId`.  
- **No** tiene `ClubId`; el club se deduce por atletas vinculados.
