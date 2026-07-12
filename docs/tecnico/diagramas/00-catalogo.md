# 00 — Catálogo (SportTrack-Sigdef API)

## Tipos

| Tipo | Categoría | Archivo |
|------|-----------|---------|
| Contexto | Obligatorio | [01](./01-globales.md#1-contexto) |
| Contenedores | Obligatorio | [01](./01-globales.md#2-contenedores) |
| Capas | Obligatorio | [01](./01-globales.md#3-capas) |
| Despliegue | Ops | [01](./01-globales.md#4-despliegue) |
| Despliegue ambientes | Opcional | [01](./01-globales.md#5-despliegue-detallado) |
| Paquetes solución | Recomendado | [01](./01-globales.md#6-paquetes) |
| Componentes API | Recomendado | [01](./01-globales.md#7-componentes) |
| Casos de uso API | Obligatorio | [02](./02-casos-actividad-estados.md) |
| Actividad | Recomendado | [02](./02-casos-actividad-estados.md) |
| Estados | Recomendado | [02](./02-casos-actividad-estados.md) |
| ER + clases dominio | Obligatorio | [03](./03-er-clases-dominio.md) |
| Clases aplicación | Opcional | [04](./04-clases-aplicacion.md) |
| Secuencia / contratos | Obligatorio | [05](./05-secuencias-api.md) |

## Dominios API

- Auth / JWT / Usuario / Auditoría  
- Federación / Club / PlanSaaS / Tenant  
- Personas SIGDEF (Atleta, Tutor, Entrenador, Delegado, Documentación)  
- Regatas (Evento → Resultado) + TimingHub  
- Mensajería + `SistemaOrigen`  
- Pagos / MercadoPago / SaaS  

## Proyectos .csproj

| Proyecto | Rol |
|----------|-----|
| `SportTrack-Sigdef` | Host API, Controllers, Program.cs |
| `SportTrack-Sigdef.Entidades` | Dominio |
| `SportTrack-Sigdef.AccesoDatos` | DbContext, migraciones, repos |
| `SportTrack-Sigdef.Controladores` | Services, Hub, Documentacion, etc. |
