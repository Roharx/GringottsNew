#!/bin/bash
set -e

echo "Running DB migrations..."

DB_HOST=${POSTGRES_HOST:-postgres}
DB_USER=${POSTGRES_USER:-gringotts}
DB_PASS=${POSTGRES_PASSWORD:-secret}
DB_NAME=${POSTGRES_DB:-$DB_USER}

psql "host=$DB_HOST user=$DB_USER password=$DB_PASS dbname=$DB_NAME" -f /app/001_init.sql

echo "Migrations completed." 

