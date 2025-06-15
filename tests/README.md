# Tests

Integration tests ensure the PostgreSQL database is set up correctly. Run `dotnet test` from the repository root to execute them. The tests expect the database from `docker-compose` to be running and accessible via the connection string in `.env`.
