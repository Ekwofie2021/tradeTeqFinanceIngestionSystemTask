# Seed Data Documentation

This folder contains seed data for initializing the Trade Finance Ingestion System with sample data.

## Overview

The `DatabaseSeeder` class automatically seeds the in-memory databases with test data when the application starts.

## How It Works

1. The seeder is called automatically in `Program.cs` during application startup
2. It checks if data already exists before seeding (idempotent)
3. Uses fixed GUIDs for reproducible test data
4. Data persists only for the lifetime of the application (in-memory database)

## Testing the Seed Data

After starting the application, you can test the seeded data with these endpoints:

```bash
# Get all notes (paginated)
GET http://localhost:5018/api/note?pageNumber=1&pageSize=10

# Get all instruments (paginated)
GET http://localhost:5018/api/instrument?pageNumber=1&pageSize=10

# Test pagination
GET http://localhost:5018/api/note?pageNumber=2&pageSize=20
GET http://localhost:5018/api/instrument?pageNumber=3&pageSize=15
```

**Note:** Since GUIDs are randomly generated on each run, you'll need to get actual IDs from the list endpoints to test individual GET by ID operations.

## Modifying Seed Data

To add or modify seed data:

1. Edit `DatabaseSeeder.cs`
2. Add new entries to the `notes` or `instruments` lists
3. Rebuild and restart the application

## Data Relationships

- Each Note can have multiple Instruments
- ASSUMPTION: All instruments for a note must use the same currency as the note
- Instruments reference their parent note via `NoteId`


1. Technologies Section
•  Detailed breakdown of all frameworks and libraries
•  Version numbers
•  Purpose of each technology

2. Architecture Overview
•  Clean Architecture layers

3. Detailed Project Structure
Each layer fully documented:

•  Presentation Layer - Controllers, DTOs, HTTP concerns
•  Application Layer - Commands, Queries, Handlers, Validators, Behaviors
•  Domain Layer - Entities, Enums, Value Objects
•  Infrastructure Layer - DbContexts, Repositories, Seed Data
•  Test Layer - 40 unit tests breakdown

4. How to Run
Three different approaches:
•  Visual Studio (F5)
•  Command Line (dotnet run)
•  Testing (dotnet test)

5. API Endpoints
•  Complete endpoint table with HTTP methods
•  Status codes
•  Example requests with JSON payloads
•  Pagination parameters

6. Testing Section
•  Test statistics (40 tests, 100% pass)
•  Commands to run specific test suites
•  Coverage breakdown

7. Design Decisions
Explains WHY each pattern was chosen:
•  Clean Architecture
•  CQRS with MediatR
•  FluentValidation
•  Repository Pattern
•  InMemory Database
•  Computed Fields
•  Value Objects

8. Additional Sections
•  Seed data details (55 notes, 61 instruments)
•  Key features checklist
•  License and author info

9. Things I would have add if I had more time:
   1. Comprehensive logging 
   2. Global error handling
   3. authentication
   4. Dockerize the solution with a REST API
