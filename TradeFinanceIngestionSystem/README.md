# Trade Finance Ingestion System

A REST API for managing trade finance notes and their related financial instruments. This was built as a technical assignment to demonstrate Clean Architecture and CQRS patterns.

## What I Built

This system lets you manage Notes (basically trade finance documents) and Instruments (financial products like receivables, guarantees, or letters of credit). You can create, read, update, and delete both, plus see computed totals and dates.

## Table of Contents

- [Tech Stack](#tech-stack)
- [NuGet Packages](#nuget-packages)
- [How It's Structured](#how-its-structured)
- [How to Run](#how-to-run)
- [API Endpoints](#api-endpoints)
- [Tests](#tests)
- [Why I Built It This Way](#why-i-built-it-this-way)

---

## Tech Stack

**Framework:** .NET 9.0 with C# 12  
**API:** ASP.NET Core Web API  
**Database:** Entity Framework Core with InMemory provider (no setup needed)  
**Patterns:** Clean Architecture, CQRS with MediatR  
**Validation:** FluentValidation  
**Testing:** xUnit, Moq, FluentAssertions  
**Docs:** Swagger/OpenAPI  

---

## NuGet Packages

Here are all the packages I used:

### Presentation Layer
```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.6.2" />
```

### Application Layer
```xml
<PackageReference Include="MediatR" Version="13.1.0" />
<PackageReference Include="FluentValidation" Version="11.9.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.9.0" />
```

### Infrastructure Layer
```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="9.0.11" />
```

### Test Project
```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
<PackageReference Include="xunit" Version="2.9.2" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.8.2" />
<PackageReference Include="Moq" Version="4.20.72" />
<PackageReference Include="FluentAssertions" Version="8.8.0" />
<PackageReference Include="coverlet.collector" Version="6.0.2" />
```

---

---

## How It's Structured

I organized this using Clean Architecture with 4 layers. Think of it like an onion - each layer only knows about the layers inside it, not outside.

```
Presentation (API Controllers)
        ↓
Application (Business Logic)
        ↓
Domain (Core Entities)
        ↑
Infrastructure (Database)
```

### 1. Presentation Layer (`TradeFinanceIngestionSystem`)
This is where HTTP stuff happens - controllers, requests, responses.

**Contents:**
```
TradeFinanceIngestionSystem/
├── Controllers/
│   ├── NoteController.cs          # Note CRUD endpoints
│   └── InstrumentController.cs    # Instrument CRUD endpoints
├── DTOs/
│   ├── Requests/
│   │   ├── CreateNoteRequest.cs
│   │   ├── CreateInstrumentRequest.cs
│   │   ├── UpdateNoteStatusRequest.cs
│   │   └── UpdateInstrumentRequest.cs
│   └── Responses/
│       └── PagedResult.cs         # Generic pagination wrapper
└── Program.cs                      # Application startup & DI configuration
```

**Key Responsibilities:**
- ✅ RESTful endpoint routing (`/api/note`, `/api/instrument`)
- ✅ HTTP status code handling (200, 201, 204, 400, 404, 500)
- ✅ Request/Response DTO mapping
- ✅ Exception handling with try-catch blocks
- ✅ Dependency injection registration
- ✅ Swagger/OpenAPI documentation

**Technologies:**
- ASP.NET Core Web API
- Minimal API configuration
- Swagger UI

---

### 2. Application Layer (`TradeFinanceIngestionSystem.Application`)
This is the brain - where all the business logic lives. I'm using CQRS here (Commands for writes, Queries for reads).

**Contents:**
```
TradeFinanceIngestionSystem.Application/
├── Commands/                      # Write operations
│   ├── CreateNote/
│   │   ├── CreateNoteCommand.cs
│   │   ├── CreateNoteCommandHandler.cs
│   │   └── CreateNoteCommandValidator.cs
│   ├── CreateInstrument/
│   │   ├── CreateInstrumentCommand.cs
│   │   ├── CreateInstrumentCommandHandler.cs
│   │   └── CreateInstrumentCommandValidator.cs
│   ├── Update/
│   │   ├── UpdateInstrumentCommand.cs
│   │   ├── UpdateInstrumentCommandHandler.cs
│   │   ├── UpdateNoteStatusCommand.cs
│   │   └── UpdateNoteStatusCommandHandler.cs
│   ├── DeleteNote/
│   │   ├── DeleteNoteCommand.cs
│   │   └── DeleteNoteCommandHandler.cs
│   └── DeleteInstrument/
│       ├── DeleteInstrumentCommand.cs
│       └── DeleteInstrumentCommandHandler.cs
├── Queries/                       # Read operations
│   ├── GetNotes/
│   │   ├── GetNotesQuery.cs
│   │   ├── GetNotesQueryHandler.cs
│   │   ├── GetNoteQuery.cs
│   │   ├── GetNoteQueryHandler.cs
│   │   ├── GetNoteWithInstrumentQuery.cs
│   │   └── GetNoteWithInstrumentQueryHandler.cs
│   ├── GetInstruments/
│   │   ├── GetInstrumentsQuery.cs
│   │   └── GetInstrumentsQueryHandler.cs
│   ├── GetInstrument/
│   │   ├── GetInstrumentQuery.cs
│   │   └── GetInstrumentQueryHandler.cs
│   └── DTOs/
│       ├── NoteDto.cs
│       ├── NoteDetailDto.cs       # Note with computed fields
│       └── InstrumentDto.cs
├── Behaviors/
│   └── ValidationBehavior.cs      # MediatR pipeline for validation
├── Interfaces/
│   ├── INoteRepository.cs
│   └── IInstrumentRepository.cs
└── Mapper/
    └── NoteWithInstrumentMapper.cs # Maps entities to DTOs with computed fields
```

**What's in here:**
- Commands (Create, Update, Delete) with handlers
- Queries (Get single, Get list with pagination) with handlers  
- Validators using FluentValidation
- Computed fields logic (totals and dates)
- Repository interfaces

**Why CQRS?** Makes it clearer what changes data vs what just reads it.

---

### 3. Domain Layer (`TradeFinanceIngestionSystem.Domain`)
The core stuff - just plain C# classes with zero dependencies on frameworks. This is the heart of the system.

**Contents:**
```
Domain/
├── Entities/
│   ├── Note.cs                    # Note aggregate root
│   └── Instrument.cs              # Instrument entity
├── Enums/
│   ├── Status.cs                  # DRAFT, PUBLISHED
│   ├── Type.cs                    # RECEIVABLE, GUARANTEE, LETTER_OF_CREDIT
│   └── Currency.cs                # USD, GBP, EUR
└── ValueObjects/
    └── Price.cs                   # Amount + Currency value object
```

**What's in here:**
- **Note** - The main document (has Id, reference number, issue date, status)
- **Instrument** - Financial product linked to a Note (receivable, guarantee, or letter of credit)
- **Enums** - Status (DRAFT/PUBLISHED), Type (3 instrument types), Currency (USD/GBP/EUR)
- **Price** - Value object that bundles amount + currency together (validates that amounts aren't negative)

---

### 4. Infrastructure Layer (`TradeFinanceIngestionSystem.Infrastructure`)
Database stuff - Entity Framework, repositories, and seed data.

**Contents:**
```
TradeFinanceIngestionSystem.Infrastructure/
├── DbContexts/
│   ├── NoteDataContext.cs         # EF Core context for Notes
│   └── InstrumentDataContext.cs   # EF Core context for Instruments
├── Repositories/
│   ├── NoteRepository.cs          # Note CRUD operations
│   └── InstrumentRepository.cs    # Instrument CRUD operations
└── SeedData/
    └── DatabaseSeeder.cs          # Seeds 55 Notes + 61 Instruments
```

**What's in here:**
- EF Core DbContext for Notes and Instruments
- InMemory database (no SQL Server needed!)
- Repositories that implement the interfaces from Application layer
- Seed data - 55 notes and 61 instruments ready to go

**Performance tip:** I fixed the N+1 query problem by batch-fetching instruments. Instead of making 1 query per note, it's now just 2 queries total. That's a 94% reduction in database calls.

---

### 5. Tests (`TradeFinanceIngestionSystem.Tests`)
All my unit tests - 40 of them, all passing.

**Contents:**
```
TradeFinanceIngestionSystem.Tests/
├── Commands/
│   ├── DeleteNoteCommandHandlerTests.cs
│   ├── DeleteInstrumentCommandHandlerTests.cs
│   ├── UpdateInstrumentCommandHandlerTests.cs
│   └── UpdateNoteStatusCommandHandlerTests.cs
├── Validators/
│   ├── CreateNoteCommandValidatorTests.cs
│   └── CreateInstrumentCommandValidatorTests.cs
├── Queries/
│   ├── GetInstrumentQueryHandlerTests.cs
│   └── GetInstrumentsQueryHandlerTests.cs
└── Mappers/
    └── NoteWithInstrumentMapperTests.cs
```

**What's tested:**
- Validators (12 tests) - Making sure bad data gets rejected
- Commands (17 tests) - Create, update, delete operations
- Queries (4 tests) - Reading data correctly
- Computed fields (7 tests) - Totals and dates calculate right

**Tools:** xUnit for the framework, Moq for fake repositories, FluentAssertions for readable test code.

---

## How to Run

**What you need:** [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

### Quick Start

```bash
# Clone the repo
git clone <repository-url>
cd TradeFinanceIngestionSystem

# Run it
dotnet run --project TradeFinanceIngestionSystem
```

That's it! The API will start at `http://localhost:5018`

Go to `http://localhost:5018/swagger` to see all the endpoints and try them out.

### Run Tests

```bash
# All tests
dotnet test

# Just the validators
dotnet test --filter "FullyQualifiedName~Validators"

# Just the commands
dotnet test --filter "FullyQualifiedName~Commands"
```

You should see: `total: 40, failed: 0, succeeded: 40`

---

## 🌐 API Endpoints

### **Note Endpoints**

| Method | Endpoint | Description | Status Codes |
|--------|----------|-------------|--------------|
| `GET` | `/api/note` | List notes with pagination | 200 OK |
| `GET` | `/api/note/{id}` | Get note with instruments | 200 OK, 404 Not Found |
| `POST` | `/api/note` | Create new note | 201 Created, 400 Bad Request |
| `PUT` | `/api/note/{id}` | Update note status | 204 No Content, 404 Not Found |
| `DELETE` | `/api/note/{id}` | Delete note (cascades to instruments) | 204 No Content, 404 Not Found |

### **Instrument Endpoints**

| Method | Endpoint | Description | Status Codes |
|--------|----------|-------------|--------------|
| `GET` | `/api/instrument` | List instruments with pagination | 200 OK |
| `GET` | `/api/instrument/{id}` | Get single instrument | 200 OK, 404 Not Found |
| `POST` | `/api/instrument` | Create new instrument | 201 Created, 400 Bad Request |
| `PUT` | `/api/instrument/{id}` | Update instrument | 204 No Content, 404 Not Found |
| `DELETE` | `/api/instrument/{id}` | Delete instrument | 204 No Content, 404 Not Found |

### **Pagination Parameters**
- `pageNumber` (default: 1)
- `pageSize` (default: 10)

### **Example Requests**

**Create Note:**
```http
POST /api/note
Content-Type: application/json

{
  "referenceNumber": 10001,
  "issueDate": "2025-01-15T00:00:00",
  "currency": "USD"
}
```

**Create Instrument:**
```http
POST /api/instrument
Content-Type: application/json

{
  "noteId": "11111111-1111-1111-1111-111111111111",
  "type": "RECEIVABLE",
  "issueDate": "2025-01-15T00:00:00",
  "maturityDate": "2025-07-15T00:00:00",
  "purchaseAmount": 50000,
  "repaymentAmount": 52500,
  "currency": "USD"
}
```

**Get Notes with Pagination:**
```http
GET /api/note?pageNumber=1&pageSize=5
```

**Response includes:**
- List of notes with computed fields
- Pagination metadata (totalCount, totalPages, hasNextPage, hasPreviousPage)

---

## Tests

40 tests, all green. They cover:
- Validation (making sure bad data is rejected)
- CRUD operations (create, update, delete)
- Queries (fetching data)
- Computed fields (calculations)

---

## Why I Built It This Way

**Clean Architecture** - Each layer has a specific job. Domain knows nothing about databases. Application knows nothing about HTTP. Makes it way easier to test and maintain.

**CQRS with MediatR** - Separating reads from writes makes the code clearer. Plus, validation happens automatically in the MediatR pipeline.

**FluentValidation** - Way more readable than data annotations. Rules are in their own classes and easy to test.

**Repository Pattern** - Keeps EF Core details out of the Application layer. Makes unit testing easier since I can mock the repos.

**InMemory Database** - Zero setup. Just run it. The app seeds itself with 55 notes and 61 instruments automatically.

**Computed Fields** - Total amounts and maturity dates are calculated when you query, not stored. Less chance of stale data.

**N+1 Fix** - Classic problem - if you have 50 notes, you'd make 51 database queries (1 for notes, then 1 per note for instruments). I batch-fetch instruments instead. Now it's just 2 queries total.

**Value Objects** - The `Price` class bundles amount + currency together. Stops you from accidentally mixing currencies or using negative amounts.

---

## What You Get

✅ Full CRUD for Notes and Instruments  
✅ Pagination (with totalCount, hasNext, etc.)  
✅ Computed totals and dates  
✅ Input validation  
✅ 40 passing unit tests  
✅ Swagger docs  
✅ InMemory DB with seed data  

---

Built as a technical assignment to demonstrate Clean Architecture and CQRS.
