# Infrastructure Layer Proposal for Beach Weather Station

## Purpose
The Infrastructure layer provides concrete implementations for data persistence, external integrations, and system services, based on abstractions defined in the Domain layer. It enables the application to interact with Azure Cosmos DB (via Entity Framework Core), authentication providers, logging systems, and alerting mechanisms, while keeping the core business logic isolated.

## Responsibilities
- Implement repository interfaces for data access (Cosmos DB via Entity Framework Core)
- Integrate with external services (authentication, alerting, logging)
- Manage configuration and connection details for external resources
- Ensure idempotency and data validation at the persistence level

## Key Components

### 1. Repository Implementations

For each repository interface in the Domain layer, provide an implementation that uses Entity Framework Core with the Cosmos DB provider:
- `BatteryChangeRepository` (implements `IBatteryChangeRepository`)
- `DeviceRepository` (implements `IDeviceRepository`)
- `HeartbeatRepository` (implements `IHeartbeatRepository`)
- `TemperatureReadingRepository` (implements `ITemperatureReadingRepository`)

These repositories will use a shared `DbContext` configured for Cosmos DB.

### 2. Data Models & Mapping
- Define Entity Framework Core entity classes for each aggregate/entity
- Configure entity-to-container mapping using EF Core's Cosmos DB provider
- Implement mapping logic between domain entities and EF Core entities if needed

### 3. External Services
- **Alerting:** Notify if readings are delayed (e.g., via email, webhook, or Azure Monitor)

### 4. Dependency Injection
- Register the `DbContext` (configured for Cosmos DB) and all infrastructure services and repositories in the DI container
- Use constructor injection to provide implementations to the Application layer

### 5. Configuration
- Read Cosmos DB connection strings and secrets from configuration files or environment variables
- Configure the `DbContext` for both local development and production environments

## Example Structure
```
BeachWeatherStation.Infrastructure/
  Repositories/
    BatteryChangeRepository.cs
    DeviceRepository.cs
    HeartbeatRepository.cs
    TemperatureReadingRepository.cs
  Data/
    BeachWeatherStationDbContext.cs
  Services/
    AlertService.cs
  Models/
    BatteryChangeDocument.cs
    DeviceDocument.cs
    HeartbeatDocument.cs
    TemperatureReadingDocument.cs
  Extensions/
    ServiceCollectionExtensions.cs
```

## Implementation Notes
- Use Entity Framework Core with the Cosmos DB provider
- All repository methods should be asynchronous
- Implement idempotency checks to prevent duplicate records
- Use configuration and options pattern for settings
- Use ILogger to log crital events and debug related sections.

## Benefits
- Clean separation of concerns
- Maintainable codebase
- Scalable and secure integration with Azure services
- Flexibility to swap infrastructure components without affecting business logic

---

This proposal aligns with Clean Architecture principles and the requirements outlined in the project documentation, and leverages Entity Framework Core for Cosmos DB integration. For further details or implementation examples, please request specific component designs.
