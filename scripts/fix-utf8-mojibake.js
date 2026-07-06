/**
 * Corrige mojibake UTF-8 en archivos .cs y .md del backend.
 * Ejecutar: node scripts/fix-utf8-mojibake.js
 */
import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const root = path.resolve(__dirname, '..');

const REPLACEMENTS = [
  ['Pack D\u00c3\u00bao', 'Pack D\u00fao'],
  ['d\u00c3\u00bao', 'd\u00fao'],
  ['Federaci\u00c3\u00b3n', 'Federaci\u00f3n'],
  ['federaci\u00c3\u00b3n', 'federaci\u00f3n'],
  ['Configuraci\u00c3\u00b3n', 'Configuraci\u00f3n'],
  ['configuraci\u00c3\u00b3n', 'configuraci\u00f3n'],
  ['Validaci\u00c3\u00b3n', 'Validaci\u00f3n'],
  ['validaci\u00c3\u00b3n', 'validaci\u00f3n'],
  ['revisi\u00c3\u00b3n', 'revisi\u00f3n'],
  ['Revisi\u00c3\u00b3n', 'Revisi\u00f3n'],
  ['Progresi\u00c3\u00b3n', 'Progresi\u00f3n'],
  ['progresi\u00c3\u00b3n', 'progresi\u00f3n'],
  ['Ubicaci\u00c3\u00b3n', 'Ubicaci\u00f3n'],
  ['ubicaci\u00c3\u00b3n', 'ubicaci\u00f3n'],
  ['instituci\u00c3\u00b3n', 'instituci\u00f3n'],
  ['Instituci\u00c3\u00b3n', 'Instituci\u00f3n'],
  ['suscripci\u00c3\u00b3n', 'suscripci\u00f3n'],
  ['Suscripci\u00c3\u00b3n', 'Suscripci\u00f3n'],
  ['afiliaci\u00c3\u00b3n', 'afiliaci\u00f3n'],
  ['Afiliaci\u00c3\u00b3n', 'Afiliaci\u00f3n'],
  ['inscripci\u00c3\u00b3n', 'inscripci\u00f3n'],
  ['Inscripci\u00c3\u00b3n', 'Inscripci\u00f3n'],
  ['auditor\u00c3\u00ada', 'auditor\u00eda'],
  ['Auditor\u00c3\u00ada', 'Auditor\u00eda'],
  ['Identificaci\u00c3\u00b3n', 'Identificaci\u00f3n'],
  ['identificaci\u00c3\u00b3n', 'identificaci\u00f3n'],
  ['autom\u00c3\u00a1tico', 'autom\u00e1tico'],
  ['Autom\u00c3\u00a1tico', 'Autom\u00e1tico'],
  ['autom\u00c3\u00a1ticamente', 'autom\u00e1ticamente'],
  ['est\u00c3\u00a1', 'est\u00e1'],
  ['Est\u00c3\u00a1', 'Est\u00e1'],
  ['ten\u00c3\u00a9s', 'ten\u00e9s'],
  ['est\u00c3\u00a1n', 'est\u00e1n'],
  ['\u00c3\u00banico', '\u00fanico'],
  ['\u00c3\u00banica', '\u00fanica'],
  ['\u00c3\u008dndice', '\u00cdndice'],
  ['\u00c3\u00adndice', '\u00edndice'],
  ['\u00c3\u00adndices', '\u00edndices'],
  ['\u00c3\u008dndices', '\u00cdndices'],
  ['Relaci\u00c3\u00b3n', 'Relaci\u00f3n'],
  ['relaci\u00c3\u00b3n', 'relaci\u00f3n'],
  ['tel\u00c3\u00a9fono', 'tel\u00e9fono'],
  ['Tel\u00c3\u00a9fono', 'Tel\u00e9fono'],
  ['direcci\u00c3\u00b3n', 'direcci\u00f3n'],
  ['Direcci\u00c3\u00b3n', 'Direcci\u00f3n'],
  ['inv\u00c3\u00a1lido', 'inv\u00e1lido'],
  ['Inv\u00c3\u00a1lido', 'Inv\u00e1lido'],
  ['l\u00c3\u00admite', 'l\u00edmite'],
  ['L\u00c3\u00admite', 'L\u00edmite'],
  ['l\u00c3\u00b3gica', 'l\u00f3gica'],
  ['desv\u00c3\u00ados', 'desv\u00edos'],
  ['b\u00c3\u00basqueda', 'b\u00fasqueda'],
  ['est\u00c3\u00a1ndar', 'est\u00e1ndar'],
  ['cu\u00c3\u00a1ntas', 'cu\u00e1ntas'],
  ['m\u00c3\u00a1s', 'm\u00e1s'],
  ['M\u00c3\u00a1xima', 'M\u00e1xima'],
  ['m\u00c3\u00a1ximo', 'm\u00e1ximo'],
  ['M\u00c3\u00a1ximo', 'M\u00e1ximo'],
  ['Al D\u00c3\u00ada', 'Al D\u00eda'],
  ['al d\u00c3\u00ada', 'al d\u00eda'],
  ['cambi\u00c3\u00b3', 'cambi\u00f3'],
  ['reinici\u00c3\u00b3', 'reinici\u00f3'],
  ['Notificaci\u00c3\u00b3n', 'Notificaci\u00f3n'],
  ['notificaci\u00c3\u00b3n', 'notificaci\u00f3n'],
  ['Verificaci\u00c3\u00b3n', 'Verificaci\u00f3n'],
  ['verificaci\u00c3\u00b3n', 'verificaci\u00f3n'],
  ['t\u00c3\u00a9cnica', 't\u00e9cnica'],
  ['T\u00c3\u00a9cnica', 'T\u00e9cnica'],
  ['Asignaci\u00c3\u00b3n', 'Asignaci\u00f3n'],
  ['asignaci\u00c3\u00b3n', 'asignaci\u00f3n'],
  ['At\u00c3\u00b3mico', 'At\u00f3mico'],
  ['at\u00c3\u00b3mico', 'at\u00f3mico'],
  ['v\u00c3\u00ada', 'v\u00eda'],
  ['m\u00c3\u00a9dico', 'm\u00e9dico'],
  ['M\u00c3\u00a9todos', 'M\u00e9todos'],
  ['m\u00c3\u00a9todos', 'm\u00e9todos'],
  ['Estad\u00c3\u00adsticas', 'Estad\u00edsticas'],
  ['Informaci\u00c3\u00b3n', 'Informaci\u00f3n'],
  ['administraci\u00c3\u00b3n', 'administraci\u00f3n'],
  ['Administraci\u00c3\u00b3n', 'Administraci\u00f3n'],
  ['selecci\u00c3\u00b3n', 'selecci\u00f3n'],
  ['Selecci\u00c3\u00b3n', 'Selecci\u00f3n'],
  ['N\u00c3\u00bamero', 'N\u00famero'],
  ['n\u00c3\u00bamero', 'n\u00famero'],
  ['env\u00c3\u00ada', 'env\u00eda'],
  ['aqu\u00c3\u00ad', 'aqu\u00ed'],
  ['AQU\u00c3\u008d', 'AQU\u00cd'],
  ['pas\u00c3\u00b3', 'pas\u00f3'],
  ['actualizaci\u00c3\u00b3n', 'actualizaci\u00f3n'],
  ['CORRECCI\u00c3\u0093N', 'CORRECCI\u00d3N'],
  ['ya est\u00c3\u00a1', 'ya est\u00e1'],
  ['confusi\u00c3\u00b3n', 'confusi\u00f3n'],
  ['alfanum\u00c3\u00a9ricos', 'alfanum\u00e9ricos'],
  ['descripci\u00c3\u00b3n', 'descripci\u00f3n'],
  ['Descripci\u00c3\u00b3n', 'Descripci\u00f3n'],
  ['L\u00c3\u00b3gica', 'L\u00f3gica'],
  ['Correcci\u00c3\u00b3n', 'Correcci\u00f3n'],
  ['correcci\u00c3\u00b3n', 'correcci\u00f3n'],
  ['Posici\u00c3\u00b3n', 'Posici\u00f3n'],
  ['posici\u00c3\u00b3n', 'posici\u00f3n'],
  ['m\u00c3\u00a9tricas', 'm\u00e9tricas'],
  ['cr\u00c3\u00adticas', 'cr\u00edticas'],
  ['m\u00c3\u00adnima', 'm\u00ednima'],
  ['gen\u00c3\u00a9rico', 'gen\u00e9rico'],
  ['Simplificaci\u00c3\u00b3n', 'Simplificaci\u00f3n'],
  ['RECOMENDACI\u00c3\u201cN', 'RECOMENDACI\u00d3N'],
  ['encontr\u00c3\u00b3', 'encontr\u00f3'],
  ['Autom\u00c3\u00a1ticamente', 'Autom\u00e1ticamente'],
  ['espec\u00c3\u00adfica', 'espec\u00edfica'],
  ['vac\u00c3\u00adas', 'vac\u00edas'],
  ['par\u00c3\u00a1metros', 'par\u00e1metros'],
  ['superposici\u00c3\u00b3n', 'superposici\u00f3n'],
  ['categor\u00c3\u00ada', 'categor\u00eda'],
  ['CORRECCI\u00c3\u201cN', 'CORRECCI\u00d3N'],
  ['M\u00c3\u00a9todo', 'M\u00e9todo'],
  ['Navegaci\u00c3\u00b3n', 'Navegaci\u00f3n'],
  ['Sincronizaci\u00c3\u00b3n', 'Sincronizaci\u00f3n'],
  ['transacci\u00c3\u00b3n', 'transacci\u00f3n'],
  ['\u00e2\u0161\u00a0\u00ef\u00b8\u008f', '\u26a0\ufe0f'],
  ['\u00e2\u201e\u00b9\u00ef\u00b8\u008f', '\u2139\ufe0f'],
  ['\u00e2\u0153\u2026', '\u2705'],
];

const SKIP = new Set(['bin', 'obj', 'node_modules', '.git']);

function walk(dir, files = []) {
  for (const entry of fs.readdirSync(dir, { withFileTypes: true })) {
    if (SKIP.has(entry.name)) continue;
    const full = path.join(dir, entry.name);
    if (entry.isDirectory()) walk(full, files);
    else if (/\.(cs|md)$/i.test(entry.name)) files.push(full);
  }
  return files;
}

let fixed = 0;
for (const file of walk(root)) {
  if (file.includes(`${path.sep}scripts${path.sep}`)) continue;
  let text = fs.readFileSync(file, 'utf8');
  const orig = text;
  for (const [from, to] of REPLACEMENTS) {
    text = text.split(from).join(to);
  }
  if (text !== orig) {
    fs.writeFileSync(file, text, 'utf8');
    fixed++;
  }
}
console.log(`Archivos corregidos: ${fixed}`);
