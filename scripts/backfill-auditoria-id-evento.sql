-- Backfill IdEvento / IdEventoPrueba en Auditoria (registros previos al deploy con scope por evento).
-- Ejecutar en pgAdmin DESPUÉS de la migración AddAuditoriaEventoScope.

-- 1) Acciones de competencia con fase en el detalle: "(ID: 37)"
UPDATE "Auditoria" a
SET
    "IdEvento" = scope."IdEvento",
    "IdEventoPrueba" = scope."IdEventoPrueba"
FROM (
    SELECT
        a2."Id" AS audit_id,
        ep."IdEvento",
        ep."IdEventoPrueba"
    FROM "Auditoria" a2
    CROSS JOIN LATERAL (
        SELECT (regexp_match(a2."Detalle", '\(ID:\s*(\d+)\)', 'i'))[1]::int AS fase_id
    ) parsed
    JOIN "Fases" f ON f."Id" = parsed.fase_id
    JOIN "Etapas" e ON e."Id" = f."EtapaId"
    JOIN "EventoPruebas" ep ON ep."IdEventoPrueba" = e."EventoPruebaId"
    WHERE a2."IdEvento" IS NULL
      AND parsed.fase_id IS NOT NULL
) scope
WHERE a."Id" = scope.audit_id;

-- 2) Sorteos / promoción: "Prueba ID 123"
UPDATE "Auditoria" a
SET
    "IdEvento" = ep."IdEvento",
    "IdEventoPrueba" = ep."IdEventoPrueba"
FROM (
    SELECT
        a2."Id" AS audit_id,
        (regexp_match(a2."Detalle", 'Prueba ID\s*(\d+)', 'i'))[1]::int AS prueba_id
    FROM "Auditoria" a2
    WHERE a2."IdEvento" IS NULL
) parsed
JOIN "EventoPruebas" ep ON ep."IdEventoPrueba" = parsed.prueba_id
WHERE a."Id" = parsed.audit_id
  AND parsed.prueba_id IS NOT NULL;

-- 3) Eventos CRUD: "Evento ... (ID: 9)" o similar en UPDATE/DELETE
UPDATE "Auditoria" a
SET "IdEvento" = parsed.evento_id
FROM (
    SELECT
        a2."Id" AS audit_id,
        (regexp_match(a2."Detalle", '\(ID:\s*(\d+)\)', 'i'))[1]::int AS evento_id
    FROM "Auditoria" a2
    WHERE a2."IdEvento" IS NULL
      AND a2."Modulo" = 'Eventos'
) parsed
WHERE a."Id" = parsed.audit_id
  AND parsed.evento_id IS NOT NULL;

-- Verificación rápida (evento 9 / fase 37)
SELECT "Id", "Fecha", "Accion", "Usuario", "IdEvento", "IdEventoPrueba", left("Detalle", 80)
FROM "Auditoria"
WHERE "IdEvento" = 9 OR "Detalle" ILIKE '%ID: 37%'
ORDER BY "Fecha" DESC
LIMIT 30;
