#!/bin/bash
set -e

host="$1"
shift

echo "⏳ Waiting for RabbitMQ ($host:5672)..."
until nc -z "$host" 5672; do
  sleep 1
done

echo "✅ RabbitMQ is up - executing command"
exec "$@"
