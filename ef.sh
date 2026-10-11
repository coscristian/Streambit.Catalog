#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT="$ROOT/Streambit.Dal/Streambit.Dal.csproj"
STARTUP="$ROOT/Streambit.Api/Streambit.Api.csproj"

usage() {
  cat <<'EOF'
Uso:
  ./ef.sh add <Nombre>      Crea una migración
  ./ef.sh update [Nombre]   Aplica las migraciones pendientes, o hasta Nombre
  ./ef.sh list              Lista las migraciones
  ./ef.sh remove            Elimina la última migración que aún no se aplicó

La cadena de conexión sale de ConnectionStrings:Default
(appsettings o la variable ConnectionStrings__Default).
EOF
}

if [[ ! -f "$PROJECT" || ! -f "$STARTUP" ]]; then
  echo "No se encontraron los proyectos de EF junto a este script." >&2
  exit 1
fi

ef() {
  dotnet ef "$@" --project "$PROJECT" --startup-project "$STARTUP"
}

command="${1:-}"
if [[ -z "$command" ]]; then
  usage
  exit 1
fi
shift

case "$command" in
  add)
    if [[ $# -lt 1 || -z "${1:-}" ]]; then
      echo "Indica el nombre de la migración: ./ef.sh add NombreDeLaMigracion" >&2
      exit 1
    fi
    ef migrations add "$@"
    ;;
  update)
    ef database update "$@"
    ;;
  list)
    ef migrations list "$@"
    ;;
  remove)
    ef migrations remove "$@"
    ;;
  -h|--help|help)
    usage
    ;;
  *)
    echo "Comando desconocido: $command" >&2
    usage
    exit 1
    ;;
esac
