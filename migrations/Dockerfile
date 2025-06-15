FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY . .
RUN chmod +x run_migrations.sh
# Use absolute path in the entrypoint to avoid path issues during startup
ENTRYPOINT ["/bin/bash", "/app/run_migrations.sh"]
