# Beach Weather Station Web API — Visual Studio Solution Structure

## Overview

This document proposes a solution and folder structure for the Beach Weather Station Web API, based on the following requirements:

- Built with .NET 9
- Azure Functions (HTTP Trigger) for API endpoints
- Entity Framework Core for Cosmos DB interactions
- Support for AI/LLM-driven query endpoints
- Code structure combines vertical slicing (feature folders) with principles of Clean Architecture
- **CQRS (Command/Query Responsibility Segregation) is NOT used**; instead, use straightforward service/repository patterns.
- **API Contracts (Request/Response objects) are defined within the Functions project. No separate Contracts project.**

---

## Recommended Solution Structure

```
BeachWeatherStation.WebApi.sln
│
├── src/
│   ├── BeachWeatherStation.Functions/            # Azure Functions project (entry points, contracts, minimal logic)
│   │   ├── Triggers/
│   │   │   ├── Readings/
│   │   │   │   └── PostReadingFunction.cs
│   │   │   │   └── GetReadingsFunction.cs
│   │   │   └── AI/
│   │   │       └── QueryAIInsightsFunction.cs
│   │   ├── Contracts/
│   │   │   ├── Readings/
│   │   │   │   └── PostReadingRequest.cs
│   │   │   │   └── PostReadingResponse.cs
│   │   │   │   └── GetReadingsRequest.cs
│   │   │   │   └── GetReadingsResponse.cs
│   │   │   └── AI/
│   │   │       └── AIQueryRequest.cs
│   │   │       └── AIQueryResponse.cs
│   │   ├── DependencyInjection/
│   │   ├── Program.cs
│   │   └── host.json
│   │
│   ├── BeachWeatherStation.Application/          # Application layer (business logic, service interfaces)
│   │   ├── Readings/
│   │   │   └── ReadingService.cs
│   │   ├── AI/
│   │   │   └── AIService.cs
│   │   ├── Interfaces/
│   │   │   └── IReadingService.cs
│   │   │   └── IAIService.cs
│   │
│   ├── BeachWeatherStation.Infrastructure/       # Data access, external services, EF Core, CosmosDB, LLM
│   │   ├── Persistence/
│   │   │   └── BeachWeatherDbContext.cs
│   │   │   └── ReadingsRepository.cs
│   │   ├── AI/
│   │   │   └── LLMService.cs
│   │   └── DependencyInjection/
│   │
│   ├── BeachWeatherStation.Domain/               # Domain entities, value objects, enums
│   │   ├── Entities/
│   │   │   └── Reading.cs
│   │   └── ValueObjects/
│   │
├── tests/
│   ├── BeachWeatherStation.Application.Tests/
│   ├── BeachWeatherStation.Infrastructure.Tests/
│   └── BeachWeatherStation.Functions.Tests/
│
└── README.md
```

---

## Structure Rationale

### 1. **Contracts in Functions Project**
- API contract objects (Requests/Responses) are defined under `BeachWeatherStation.Functions/Contracts/`.
- Organized by feature: e.g., `Readings/PostReadingRequest.cs`, `Readings/PostReadingResponse.cs`.
- No need for a separate Contracts/DTOs project or folder.

### 2. **Azure Functions Project**
- Contains only trigger classes, contract objects, and minimal wiring.
- Each HTTP endpoint is a class in `Triggers/`, organized by feature (vertical slice).

### 3. **Application Layer**
- Encapsulates core business logic and service interfaces.
- Feature folders (e.g., `Readings`, `AI`) keep related logic together.
- Services expose standard methods (e.g., `AddReading`, `GetReadings`, `QueryAIInsights`).

### 4. **Infrastructure Layer**
- Implements external dependencies: EF Core (Cosmos DB), AI/LLM integration.
- Registered via Dependency Injection, interfaces in the Application layer.

### 5. **Domain Layer**
- Contains domain models, value objects, and business rules.

### 6. **Tests**
- Separate test projects for Application, Infrastructure, and Functions.

---

## Additional Recommendations

- Use Dependency Injection for all services.
- Apply configuration via `appsettings.json` or Azure App Settings.
- Use EF Core migrations for Cosmos DB data model evolution.
- Implement validation and error handling at Application and API boundaries.
- **Request/Response objects** should be as flat as possible and map cleanly to/from internal models and services.

---

**This structure keeps API contracts close to their usage, ensures clarity, and supports maintainable and scalable development for the Beach Weather Station Web API.**