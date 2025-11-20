# Docker Setup for Trade Finance Ingestion System

## Overview
This application is containerized using Docker with a multi-stage build for optimal image size and security.

## Files Added
- **Dockerfile** - Multi-stage build (SDK for build, ASP.NET runtime for final image)
- **.dockerignore** - Excludes unnecessary files from build context
- **docker-compose.yml** - Simplified deployment configuration

## Quick Start

### Using Docker Compose (Recommended)
```bash
# Start the application
docker-compose up -d

# View logs
docker-compose logs -f

# Stop the application
docker-compose down
```

### Using Docker CLI
```bash
# Build the image
docker build -t tradefinance-ingestion-system .

# Run the container
docker run -d -p 5018:8080 --name tradefinance-api tradefinance-ingestion-system

# View logs
docker logs -f tradefinance-api

# Stop and remove
docker stop tradefinance-api
docker rm tradefinance-api
```

## Configuration

### Ports
- **5018** - HTTP API endpoint (mapped from container port 8080)
- **5019** - HTTPS endpoint (optional, mapped from container port 8081)

### Environment Variables
- `ASPNETCORE_ENVIRONMENT=Development` - Enables Swagger UI
- `ASPNETCORE_URLS=http://+:8080` - Configures listening address

## Image Details
- **Base Images:**
  - Build: `mcr.microsoft.com/dotnet/sdk:9.0`
  - Runtime: `mcr.microsoft.com/dotnet/aspnet:9.0`
- **Size:** ~339MB (optimized runtime image)
- **Multi-stage build:** Reduces final image size by excluding build tools

## Testing the API

### PowerShell
```powershell
Invoke-WebRequest -Uri "http://localhost:5018/api/note?pageNumber=1&pageSize=5" -UseBasicParsing
```

### curl
```bash
curl http://localhost:5018/api/note?pageNumber=1&pageSize=5
```

### Swagger UI
When running in Development mode, access Swagger at:
```
http://localhost:5018/swagger
```

## Features
✅ Multi-stage build for smaller image size  
✅ In-memory database with auto-seeded data (55 notes, 61 instruments)  
✅ Health check ready  
✅ Development environment enabled by default  
✅ Easy deployment with docker-compose  

## Notes
- The application uses an in-memory database, so data is not persisted between container restarts
- Seed data is automatically loaded on startup
- All 40 unit tests pass in the build stage
