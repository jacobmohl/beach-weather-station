# Web API & Database



- **Entity Framework** is used for data access to Cosmos DB, following the repository pattern.
- **Azure Functions** provide the API endpoints for all required operations.
- The architecture enforces separation of concerns, testability, and scalability.

## Overview

This document outlines the technical specifications and requirements for the Beach Weather Station Web API solution as refered to as `server`.

## Platform & Tools
- **API:** Azure Functions (C#)
- **Database:** Cosmos DB (through Entity Framework)
- **Architecture:** Clean Architecture without CRQS, but influences by vertical sliging.

## Functional Requirements
- Receive temperature readings from gateway via authenticated HTTP POST

- Validate and store water temperature readings (device ID, created at timestamp, temperature and signal strengh)
- Prevent duplicate records (idempotency)
- Endpoints for:
  - Ingesting new water temperature readings inclusive signal strengh
  - Get the latest water temperature reading.
  - Get the water temperature readings for the latest 24 hour, including the higest and the lowest reading.
  - Get the average water temperature readings (average, lowest, highest) pr. day for the latest 30 days.  
  - Ingesting heartbeats from gateway
  - Ingesting battery change notifications from app/management
  - Retrieving latest heatbeats and battery changes
  - Retrieve heatbeats and battery changes for the last 24 hours
  - Retrieving historical data (by time range)
  - System health check
- Alerts if readings are delayed (no readings for 30 minutes)


## Non-Functional Requirements
- Scalable to support frequent readings
- High reliability, uptime, and data durability
- Low-latency API responses for real-time display
- Secure (HTTPS, authentication for gateway)
- Comprehensive logging and error reporting
- Use file-scoped namespaces
- Use Entity Framework to access CosmosDB through the Repository pattern
- Prefere async code (async/await)

## Solution Structure

The `server` folder implements the Beach Weather Station backend using a Clean Architecture approach. The main components are:

- **BeachWeatherStation.Application/**: Contains application logic, use cases, and service interfaces. This is where business rules and validation are implemented.
- **BeachWeatherStation.Domain/**: Defines core domain entities, value objects, domain events, and repository interfaces. This layer is independent of infrastructure and frameworks.
- **BeachWeatherStation.Infrastructure/**: Implements data access (using Entity Framework for Cosmos DB), repository patterns, and external service integrations. Contains database context and dependency injection extensions.
- **BeachWeatherStation.Worker/**: Hosts the Azure Functions endpoints. Handles HTTP triggers for ingesting readings, heartbeats, battery changes, and provides API endpoints for querying data. Contains function definitions, startup configuration, and local settings.

The solution uses the following structure:

```
server/
  BeachWeatherStation.sln                # Solution file
  src/
    BeachWeatherStation.Application/     # Application layer
    BeachWeatherStation.Domain/          # Domain layer
    BeachWeatherStation.Infrastructure/  # Infrastructure/data access
    BeachWeatherStation.Worker/          # Azure Functions host/API endpoints
```
### Example Use Case: Ingesting a Reading
1. API endpoint receives a POST request with a new water temperature reading including signal strengh.
2. The request is mapped to a `CreateTemperatureReadingDto`.
3. `ReadingService` validates the DTO using `TemperatureReadingValidator`.
4. If valid, the service interacts with the domain and repository to persist the reading.
5. The service returns a result or error to the API endpoint.

### BeachWeatherStation.Domain

Defines the core business logic and domain model for the solution. This folder contains:
- **Entities/**: Classes representing key concepts such as `Device`, `TemperatureReading`, `Heartbeat`, and `BatteryChange`.
- **Aggregates/**: Aggregate roots, e.g., `DeviceAggregate`, that encapsulate domain logic and relationships.
- **Events/**: Domain events like `TemperatureReadingReceivedEvent` and `HeartbeatReceivedEvent` for decoupled business logic.
- **Interfaces/**: Repository and service interfaces (e.g., `IDeviceRepository`, `IAlertService`) that abstract data access and domain services.
- **Services/**: Domain services implementing business rules, such as validation and alerting.
This layer is independent of infrastructure and frameworks, ensuring the core logic is reusable and testable.

### BeachWeatherStation.Application

Contains the application layer, orchestrating use cases and business processes. This folder includes:
- Application services and handlers for processing commands and queries (e.g., ingesting readings, heartbeats, battery changes).
- Validation logic and coordination between domain and infrastructure layers.
- Interfaces for dependency injection and abstraction.
This layer acts as a bridge between the domain and the outside world, ensuring business rules are enforced.

### BeachWeatherStation.Infrastructure

Implements data access, external integrations, and infrastructure concerns. This folder provides:
- **Data/**: The `BeachWeatherStationDbContext` for Entity Framework, mapping domain entities to Cosmos DB collections.
- **Repositories/**: Concrete implementations of repository interfaces for persisting and retrieving data.
- **Extensions/**: Dependency injection helpers and infrastructure configuration.
This layer connects the application to Cosmos DB and other external services, following the repository pattern for testability and separation of concerns.

### BeachWeatherStation.Worker

Hosts the Azure Functions that expose the API endpoints. This folder contains:
- Function classes (e.g., `ReadingsFunction.cs`) for HTTP triggers to ingest and query data.
- `Program.cs` for function app startup and dependency injection setup.
- `host.json` and `local.settings.json` for Azure Functions configuration.
This is the entry point for all API operations, handling requests from the gateway, app, and other clients, and coordinating with the application and domain layers.

