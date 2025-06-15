FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN chmod +x run_migrations.sh
ENTRYPOINT ["./run_migrations.sh"]
