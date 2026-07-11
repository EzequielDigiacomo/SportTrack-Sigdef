# Contratos esperados por FrontSigdef

## CU-API-01 — Listar usuarios para Gestión de Accesos

- `GET /api/Usuario` → items con `idUsuario` (camelCase), `username`, `rolFederacion`/`rol`, `idClub`, `estaActivo`.  
- Front edita password con `idUsuario` vía Auth.

## CU-API-02 — Set password admin

- `PUT /api/Auth/usuarios/{id}/password`  
- Body: string JSON de la nueva clave.  
- Id = PK `IdUsuario`.

## CU-API-03 — Atletas de club con edad

- Endpoint atletas-by-club incluye `fechaNacimiento`.  
- Front calcula menor/mayor para TutorStatusCell.

## CU-API-04 — AtletaTutor

- POST/GET/DELETE usan `participanteId` + `idTutor`.  
- Serialización camelCase estándar ASP.NET.

## CU-API-05 — Persona en detalle de tutor/atleta

- Detalle incluye objeto `participante` (nombre, apellido, documento, fecha, etc.).
