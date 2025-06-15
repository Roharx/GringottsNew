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
- `DatabaseMigrator`

## Running

```bash
docker-compose up --build
```

Copy the provided `.env` file to your environment (or adjust the values) before
running Docker Compose. It defines the shared database and RabbitMQ settings
used by several services.

This command builds all services, starts PostgreSQL, RabbitMQ, and the
monitoring stack.

## Directory Structure

- `src/` - .NET microservices
- `frontend/` - Angular frontend (placeholder)
- `src/DatabaseMigrator/` - microservice applying EF Core migrations
- `monitoring/` - Prometheus configuration
- `tests/` - xUnit integration tests validating the database schema

Each microservice is intentionally small to keep the repository clean and focused on fault isolation.

## Database Migrations

Database schema changes are managed using Entity Framework Core. The `database-migrator` service runs at startup and applies any pending migrations to the PostgreSQL database using `DbContext.Database.Migrate()`.

All services that depend on the database perform a simple connection check with retries at startup. This allows Docker Compose to bring up PostgreSQL and the services in any order.

Run integration tests to verify the schema has been applied:

```bash
dotnet test tests/DatabaseTests/DatabaseTests.csproj
```

