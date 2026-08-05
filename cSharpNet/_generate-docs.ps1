# Generate markdown study docs from .cs files
$ErrorActionPreference = "Stop"

$root = (Get-Location).Path
if (-not (Test-Path (Join-Path $root "SportTrack-Sigdef.sln"))) {
  Write-Error "Ejecutar desde la raiz del repo SportTrack-Sigdef"
}

$outRoot = Join-Path $root "cSharpNet"
$projects = @(
  "SportTrack-Sigdef",
  "SportTrack-Sigdef.AccesoDatos",
  "SportTrack-Sigdef.Controladores",
  "SportTrack-Sigdef.Entidades"
)

function Get-ConceptNotes([string]$content) {
  $notes = New-Object System.Collections.Generic.List[string]
  if ($content -match '\binterface\b') { [void]$notes.Add("- **interface**: contrato que define que debe hacer una clase, sin implementar el como.") }
  if ($content -match '\bclass\b') { [void]$notes.Add("- **class**: tipo referencia; define datos (propiedades) y comportamiento (metodos).") }
  if ($content -match '\benum\b') { [void]$notes.Add("- **enum**: conjunto de constantes con nombre (estados, tipos, roles).") }
  if ($content -match '\brecord\b') { [void]$notes.Add("- **record**: tipo inmutable orientado a datos; util para DTOs.") }
  if ($content -match 'async\s+|await\s+|\bTask<|\bTask\b') { [void]$notes.Add("- **async/await + Task**: programacion asincrona para I/O (DB, HTTP) sin bloquear hilos.") }
  if ($content -match '\[Http(Get|Post|Put|Delete|Patch)') { [void]$notes.Add("- **Atributos HTTP**: mapean metodos a rutas REST (GET/POST/PUT/DELETE).") }
  if ($content -match '\[ApiController\]|ControllerBase') { [void]$notes.Add("- **Controller ASP.NET Core**: expone endpoints; hereda de ControllerBase.") }
  if ($content -match '\[Authorize|\[AllowAnonymous') { [void]$notes.Add("- **Autorizacion**: [Authorize] exige token/rol; [AllowAnonymous] permite acceso libre.") }
  if ($content -match 'DbContext|DbSet<') { [void]$notes.Add("- **EF Core**: DbContext / DbSet<T> conectan el modelo C# con tablas SQL.") }
  if ($content -match 'IQueryable|Include\(|ToListAsync|FirstOrDefaultAsync') { [void]$notes.Add("- **LINQ + EF**: consultas en C# que se traducen a SQL.") }
  if ($content -match 'get;\s*set;|get;\s*init;') { [void]$notes.Add("- **Propiedades auto-implementadas**: el compilador crea el campo privado (get; set;).") }
  if ($content -match '\?') { [void]$notes.Add("- **Nullable**: string? / int? indican que el valor puede ser null.") }
  if ($content -match '\[Required\]|\[MaxLength|\[Key\]|\[ForeignKey|\[Column') { [void]$notes.Add("- **Data Annotations**: metadatos de validacion y mapeo a BD ([Required], [Key], etc.).") }
  if ($content -match 'IServiceCollection|AddScoped|AddSingleton|AddTransient|builder\.Services') { [void]$notes.Add("- **DI**: el contenedor crea e inyecta servicios por constructor.") }
  if ($content -match 'IMiddleware|InvokeAsync|RequestDelegate') { [void]$notes.Add("- **Middleware**: componentes encadenados en el pipeline HTTP.") }
  if ($content -match 'MapHub|Hub\b') { [void]$notes.Add("- **SignalR Hub**: comunicacion en tiempo real (WebSockets).") }
  if ($content -match 'IMapper|CreateMap|Profile') { [void]$notes.Add("- **AutoMapper**: mapea entidades a DTOs.") }
  if ($content -match 'CancellationToken') { [void]$notes.Add("- **CancellationToken**: permite cancelar operaciones async.") }
  if ($content -match 'IOptions<|IConfiguration') { [void]$notes.Add("- **Options / Configuration**: lee settings desde appsettings.json.") }
  if ($content -match 'override\s+') { [void]$notes.Add("- **override**: redefine un metodo virtual/abstract de la clase base.") }
  if ($notes.Count -eq 0) { [void]$notes.Add("- Revisa el archivo fuente: tipos, miembros y atributos entre corchetes [].") }
  return ($notes -join "`n")
}

function Get-TypeKind([string]$content) {
  if ($content -match '(?m)^\s*public\s+(partial\s+)?(static\s+)?interface\s+') { return "interface" }
  if ($content -match '(?m)^\s*public\s+(partial\s+)?(static\s+)?enum\s+') { return "enum" }
  if ($content -match '(?m)^\s*public\s+(partial\s+)?(sealed\s+)?record\s+') { return "record" }
  if ($content -match '(?m)^\s*public\s+(partial\s+)?(static\s+)?(abstract\s+)?class\s+') { return "class" }
  return "tipo"
}

function Extract-Namespace([string]$content) {
  if ($content -match 'namespace\s+([\w\.]+)') { return $Matches[1] }
  return "(sin namespace explicito / file-scoped)"
}

function Extract-Usings([string]$content) {
  $usings = [regex]::Matches($content, '(?m)^using\s+([\w\.]+)\s*;') | ForEach-Object { $_.Groups[1].Value }
  if ($usings.Count -eq 0) { return "_Ninguno o implicitos globales (GlobalUsings)._" }
  return (($usings | ForEach-Object { "- ``$_``" }) -join "`n")
}

function Extract-Attributes([string]$content) {
  $attrs = [regex]::Matches($content, '\[([A-Za-z_][\w]*(?:\([^\]]*\))?)\]') |
    ForEach-Object { $_.Groups[1].Value } |
    Select-Object -Unique
  if ($attrs.Count -eq 0) { return "_Sin atributos destacados._" }

  $known = @{
    "Key" = "Clave primaria (EF Core / DataAnnotations)."
    "Required" = "Campo obligatorio (validacion y/o BD NOT NULL)."
    "MaxLength" = "Longitud maxima de string."
    "MinLength" = "Longitud minima de string."
    "StringLength" = "Rango de longitud de string."
    "ForeignKey" = "Indica la FK de una relacion."
    "Column" = "Configura nombre/tipo de columna."
    "Table" = "Nombre de tabla en BD."
    "NotMapped" = "Propiedad no se persiste en BD."
    "JsonIgnore" = "No se serializa a JSON."
    "ApiController" = "Convenciones de API (validacion automatica del modelo)."
    "Route" = "Plantilla de ruta del controller/accion."
    "HttpGet" = "Endpoint HTTP GET (lectura)."
    "HttpPost" = "Endpoint HTTP POST (creacion)."
    "HttpPut" = "Endpoint HTTP PUT (actualizacion)."
    "HttpDelete" = "Endpoint HTTP DELETE (baja)."
    "HttpPatch" = "Endpoint HTTP PATCH (actualizacion parcial)."
    "Authorize" = "Requiere autenticacion (y opcionalmente roles/policies)."
    "AllowAnonymous" = "Permite acceso sin autenticacion."
    "FromBody" = "Parametro desde el body JSON."
    "FromQuery" = "Parametro desde la query string."
    "FromRoute" = "Parametro desde la ruta URL."
    "FromForm" = "Parametro desde form-data (uploads)."
    "FromServices" = "Resuelve el parametro desde DI."
    "ProducesResponseType" = "Documenta codigos de respuesta (Swagger)."
    "Consumes" = "Content-Type que acepta la accion."
    "Produces" = "Content-Type que produce la accion."
  }

  $sb = New-Object System.Collections.Generic.List[string]
  foreach ($a in $attrs) {
    $name = ($a -split '\(')[0]
    $desc = if ($known.ContainsKey($name)) { $known[$name] } else { "Atributo de metadatos aplicado al tipo o miembro." }
    [void]$sb.Add("- ``[$a]`` - $desc")
  }
  return ($sb -join "`n")
}

function Extract-TypeNames([string]$content) {
  $names = New-Object System.Collections.Generic.List[string]
  $patterns = @(
    '(?m)^\s*(?:public|internal)\s+(?:partial\s+)?(?:static\s+)?(?:abstract\s+)?(?:sealed\s+)?class\s+(\w+)',
    '(?m)^\s*(?:public|internal)\s+(?:partial\s+)?interface\s+(\w+)',
    '(?m)^\s*(?:public|internal)\s+(?:partial\s+)?enum\s+(\w+)',
    '(?m)^\s*(?:public|internal)\s+(?:partial\s+)?(?:sealed\s+)?record\s+(\w+)'
  )
  foreach ($p in $patterns) {
    [regex]::Matches($content, $p) | ForEach-Object { [void]$names.Add($_.Groups[1].Value) }
  }
  return ($names | Select-Object -Unique)
}

function Extract-Properties([string]$content) {
  $props = [regex]::Matches(
    $content,
    '(?m)^\s*(?:public|protected|internal|private)\s+(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:required\s+)?([\w\.<>,\?\[\]]+)\s+(\w+)\s*\{\s*get\s*;(?:\s*(?:set|init)\s*;)?\s*\}'
  )
  if ($props.Count -eq 0) { return "_Sin propiedades auto-implementadas detectadas (o son de otro estilo)._" }
  $rows = New-Object System.Collections.Generic.List[string]
  [void]$rows.Add("| Propiedad | Tipo | Notas |")
  [void]$rows.Add("|-----------|------|-------|")
  foreach ($m in $props) {
    $type = $m.Groups[1].Value
    $name = $m.Groups[2].Value
    $nullable = if ($type -match '\?') { "Puede ser null" } else { "No-null (segun anotaciones)" }
    [void]$rows.Add("| ``$name`` | ``$type`` | $nullable |")
  }
  return ($rows -join "`n")
}

function Extract-Methods([string]$content) {
  $methods = [regex]::Matches(
    $content,
    '(?m)^\s*(?:public|protected|internal|private)\s+(?:static\s+)?(?:async\s+)?(?:virtual\s+)?(?:override\s+)?(?:abstract\s+)?([\w\.<>,\?\[\]]+)\s+(\w+)\s*\(([^)]*)\)'
  )
  $rows = New-Object System.Collections.Generic.List[string]
  [void]$rows.Add("| Metodo | Retorno | Parametros | Async |")
  [void]$rows.Add("|--------|---------|------------|-------|")
  $count = 0
  $skip = @('get','set','init','class','interface','enum','record','if','switch','for','foreach','using','return','new','where','select')
  foreach ($m in $methods) {
    $ret = $m.Groups[1].Value
    $name = $m.Groups[2].Value
    $pars = $m.Groups[3].Value.Trim()
    if ($skip -contains $ret) { continue }
    if ($name -match '^(get_|set_|if|for)$') { continue }
    $isAsync = if ($m.Value -match '\basync\b' -or $ret -match '^Task') { "Si" } else { "No" }
    if ([string]::IsNullOrWhiteSpace($pars)) { $pars = "-" } else { $pars = ($pars -replace '\s+', ' ') }
    [void]$rows.Add("| ``$name`` | ``$ret`` | ``$pars`` | $isAsync |")
    $count++
  }
  if ($count -eq 0) { return "_Sin metodos publicos/protegidos detectados (puede ser solo DTO/entidad)._" }
  return ($rows -join "`n")
}

function Extract-EnumMembers([string]$content) {
  if ($content -notmatch '\benum\b') { return $null }
  $bodyMatch = [regex]::Match($content, 'enum\s+\w+[^{]*\{([^}]+)\}', 'Singleline')
  if (-not $bodyMatch.Success) { return "_No se pudo extraer el cuerpo del enum._" }
  $members = $bodyMatch.Groups[1].Value -split ',' |
    ForEach-Object { ($_ -replace '//.*$','').Trim() } |
    Where-Object { $_ -ne '' }
  return (($members | ForEach-Object { "- ``$_``" }) -join "`n")
}

function New-DocMarkdown([string]$relPath, [string]$content, [string]$project) {
  $fileName = [IO.Path]::GetFileName($relPath)
  $typeNames = @(Extract-TypeNames $content)
  $typeList = if ($typeNames.Count) { ($typeNames | ForEach-Object { "``$_``" }) -join ", " } else { "(ver fuente)" }
  $kind = Get-TypeKind $content
  $ns = Extract-Namespace $content
  $usings = Extract-Usings $content
  $attrs = Extract-Attributes $content
  $props = Extract-Properties $content
  $methods = Extract-Methods $content
  $concepts = Get-ConceptNotes $content
  $enumMembers = Extract-EnumMembers $content
  $srcPath = "$project/$($relPath -replace '\\','/')"

  $md = @"
# $fileName

> Fuente: ``$srcPath``

## Que es este archivo

Archivo C# del proyecto **$project**. Define: $typeList (tipo principal: **$kind**).
Sirve como material de estudio del codigo real de SportTrack-Sigdef.

## Conceptos C# / .NET que aparecen

$concepts

## Namespace

``````
$ns
``````

## Usings (importaciones)

$usings

## Atributos detectados

$attrs

## Propiedades

$props

## Metodos

$methods
"@

  if ($enumMembers) {
    $md += "`n`n## Miembros del enum`n`n$enumMembers"
  }

  $md += @"


## Como estudiarlo

1. Abre el ``.cs`` original en el IDE.
2. Identifica el tipo ($kind) y su responsabilidad.
3. Lee cada propiedad: tipo, nullabilidad y significado de negocio.
4. Si hay metodos, sigue el flujo (validaciones -> persistencia -> retorno DTO).
5. Busca en este mismo directorio los tipos relacionados (DTOs, interfaces, entidades).

## Notas de estudio

- En C#, casi todo vive dentro de un **tipo** (class / interface / enum / record).
- Los corchetes ``[Atributo]`` agregan metadatos usados por el runtime, EF Core, ASP.NET, Swagger, etc.
- ``Task`` / ``async`` aparecen cuando hay trabajo de I/O (base de datos, HTTP, archivos).
- Las interfaces (``I...``) desacoplan el contrato de la implementacion (util para testing y DI).
"@

  return $md
}

$created = 0
foreach ($project in $projects) {
  $projPath = Join-Path $root $project
  if (-not (Test-Path $projPath)) { Write-Warning "No existe $projPath"; continue }

  $csFiles = Get-ChildItem -Path $projPath -Recurse -Filter *.cs -File |
    Where-Object { $_.FullName -notmatch '\\(obj|bin|\.vs|Migrations)\\' }

  foreach ($f in $csFiles) {
    $rel = $f.FullName.Substring($projPath.Length).TrimStart('\')
    $parent = Split-Path $rel -Parent
    $outDir = if ([string]::IsNullOrEmpty($parent)) {
      Join-Path $outRoot $project
    } else {
      Join-Path (Join-Path $outRoot $project) $parent
    }
    if (-not (Test-Path $outDir)) { New-Item -ItemType Directory -Force -Path $outDir | Out-Null }
    $outFile = Join-Path $outDir ([IO.Path]::GetFileNameWithoutExtension($rel) + ".md")

    # Keep richer docs already written by agents (> 2.5 KB)
    if (Test-Path $outFile) {
      $existing = Get-Item $outFile
      if ($existing.Length -gt 2500) { continue }
    }

    $content = Get-Content -Path $f.FullName -Raw -ErrorAction SilentlyContinue
    if ($null -eq $content) { $content = "" }
    $md = New-DocMarkdown -relPath $rel -content $content -project $project
    [System.IO.File]::WriteAllText($outFile, $md, [System.Text.UTF8Encoding]::new($false))
    $created++
  }

  $projReadme = Join-Path (Join-Path $outRoot $project) "README.md"
  if (-not (Test-Path $projReadme)) {
    $count = ($csFiles | Measure-Object).Count
    $readme = @"
# $project

Documentacion de estudio del proyecto ``$project``.

Hay **$count** archivos ``.cs`` documentados (espejando la estructura del codigo fuente).
Abre cada ``.md`` junto a su ``.cs`` correspondiente.
"@
    [System.IO.File]::WriteAllText($projReadme, $readme, [System.Text.UTF8Encoding]::new($false))
  }
}

Write-Host "Documentos generados/actualizados: $created"
Write-Host "Salida: $outRoot"
