# Application Layer Implementation Proposal

This document proposes the structure and key components for the `BeachWeatherStation.Application` project, based on the requirements and architecture described in the solution documentation.

## Purpose
The Application layer orchestrates use cases and business processes, acting as a bridge between the domain and the outside world. It enforces business rules, coordinates validation, and manages the flow of data between the API endpoints and the domain/infrastructure layers.


- Handle application use cases (e.g., ingest readings, heartbeats, battery changes)
- Coordinate validation and business logic
- Interact with domain entities and services
- Abstract dependencies for testability


## Proposed Structure (No CQRS)

```
BeachWeatherStation.Application/
  ├── Services/
  │     ├── TemperatureReadingService.cs
  │     ├── HeartbeatService.cs
  │     └── BatteryChangeService.cs
  ├── DTOs/
  │     ├── CreateTemperatureReadingDto.cs
  │     ├── CreateHeartbeatDto.cs
  │     └── CreateBatteryChangeDto.cs
  ├── Interfaces/
  │     ├── IReadingService.cs
  │     ├── IHeartbeatService.cs
  │     └── IBatteryChangeService.cs
  └── Validators/
        └── TemperatureReadingValidator.cs
```



## Implementation Notes
- **Services:** Application services encapsulate business use cases, orchestrate domain logic, and interact with repositories (repository interfaces are defined in the Domain layer). For example, `ReadingService` handles ingesting, validating, and retrieving readings.
- **DTOs:** Data Transfer Objects for moving data between layers and for API responses.
- **Interfaces:** Abstractions for application services, supporting dependency injection and testability.
- **Validators:** Ensure incoming data meets business and domain requirements before processing.


## Example Use Case: Ingesting a Reading
1. API endpoint receives a POST request with a new reading.
2. The request is mapped to a `ReadingDto`.
3. `ReadingService` validates the DTO using `ReadingValidator`.
4. If valid, the service interacts with the domain and repository to persist the reading.
5. The service returns a result or error to the API endpoint.


## Benefits
- Simple, maintainable structure without CQRS complexity
- Clear separation of concerns and testable components
- Scalable and consistent with Clean Architecture principles

---
This proposal provides a foundation for implementing the application layer to meet the solution's requirements and support robust, maintainable development.
