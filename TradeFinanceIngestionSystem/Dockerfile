# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY TradeFinanceIngestionSystem.sln .

# Copy project files
COPY Domain/TradeFinanceIngestionSystem.Domain.csproj Domain/
COPY TradeFinanceIngestionSystem.Application/TradeFinanceIngestionSystem.Application.csproj TradeFinanceIngestionSystem.Application/
COPY TradeFinanceIngestionSystem.Infrastructure/TradeFinanceIngestionSystem.Infrastructure.csproj TradeFinanceIngestionSystem.Infrastructure/
COPY TradeFinanceIngestionSystem/TradeFinanceIngestionSystem.csproj TradeFinanceIngestionSystem/
COPY TradeFinanceIngestionSystem.Tests/TradeFinanceIngestionSystem.Tests.csproj TradeFinanceIngestionSystem.Tests/

# Restore dependencies
RUN dotnet restore TradeFinanceIngestionSystem.sln

# Copy all source code
COPY . .

# Build the application
WORKDIR /src/TradeFinanceIngestionSystem
RUN dotnet build -c Release -o /app/build --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "TradeFinanceIngestionSystem.dll"]
