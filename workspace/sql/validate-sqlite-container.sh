#!/usr/bin/env bash
set -eu

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
schema_path="$repo_root/workspace/sql/sqlite/schema.sql"

if ! command -v docker >/dev/null 2>&1; then
    echo "Docker is required for SQLite container validation." >&2
    exit 1
fi

docker run --rm \
    -v "$schema_path:/schema.sql:ro" \
    alpine:3.20 \
    sh -c 'apk add --no-cache sqlite >/dev/null && sqlite3 /tmp/patterning.db < /schema.sql && test "$(sqlite3 /tmp/patterning.db "SELECT COUNT(*) FROM sqlite_master WHERE type = '\''table'\'' AND name NOT LIKE '\''sqlite_%'\'';")" = "9" && sqlite3 /tmp/patterning.db "SELECT name FROM sqlite_master WHERE type = '\''table'\'' AND name NOT LIKE '\''sqlite_%'\'' ORDER BY name;"'
