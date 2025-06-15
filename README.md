# Gringotts - Fantasy Banking Microservices

This repository contains a skeleton implementation of the Gringotts banking application built with a microservice architecture.

## Stack
- **Backend:** .NET 8
- **Frontend:** Angular (served using a simple Node HTTP server)
- **Database:** PostgreSQL
- **Messaging:** RabbitMQ
- **Monitoring:** Jaeger, Prometheus, Grafana, OpenTelemetry
- **Orchestration:** Docker Compose

## Services
- `UsersService`
- `TransactionsPublisher`
- `TransactionsConsumer`
- `RecurringTransactionsService`
- `CurrencyConversionService`
- `ExchangeRateService`
- `ApiGateway`
- `Frontend`
- `Migrations`

## Running

```bash
docker-compose up --build
```

This will build all services, start PostgreSQL, RabbitMQ, and the monitoring stack.

## Directory Structure

- `src/` - .NET microservices
- `frontend/` - Angular frontend (placeholder)
- `migrations/` - database migration scripts
- `monitoring/` - Prometheus configuration
- `tests/` - placeholder for unit and integration tests

Each microservice is intentionally small to keep the repository clean and focused on fault isolation.

## Database Migrations

Database schema migrations are located in the `migrations/` directory. The `migrations` container uses a simple shell script to apply the SQL script against the PostgreSQL service when `docker-compose up` is run. The default script `001_init.sql` creates the initial tables used by all services.

