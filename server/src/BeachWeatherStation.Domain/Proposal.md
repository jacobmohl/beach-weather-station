# Proposal for Domain Layer in BeachWeatherStation.Domain

## Overview
The domain layer will encapsulate the core business logic and rules for the Beach Weather Station system. It will serve as the foundation for the application layer and ensure that the system adheres to clean architecture principles.

## Key Responsibilities
1. **Entities:** Define the core entities such as `Reading`, `Heartbeat`, `BatteryChange`, and `Device`. These entities will represent the main data structures used throughout the system.
2. **Aggregates:** Create aggregates to group related entities and enforce consistency within the domain. For example, a `Device` aggregate may include `Readings`, `Heartbeats`, and `BatteryChanges`.
3. **Domain Services:** Provide domain services for complex business logic that cannot be encapsulated within a single entity or aggregate. Examples include:
   - Validating readings for duplicates (idempotency).
   - Generating alerts for delayed readings.
   - Processing natural language queries for AI insights.
4. **Events:** Define domain events to capture significant occurrences within the system, such as `ReadingReceived`, `HeartbeatReceived`, and `BatteryChangeNotified`.
5. **Repositories:** Specify interfaces for repositories to abstract data persistence. These interfaces will be implemented in the infrastructure layer.

## Design Principles
- **Encapsulation:** Ensure that all business rules and logic are encapsulated within the domain layer.
- **Consistency:** Use aggregates to enforce consistency boundaries.
- **Separation of Concerns:** Keep the domain layer independent of external frameworks and technologies.
- **Testability:** Design the domain layer to be easily testable with unit tests.

## Proposed Structure
```
BeachWeatherStation.Domain/
├── Entities/
│   ├── Reading.cs
│   ├── Heartbeat.cs
│   ├── BatteryChange.cs
│   ├── Device.cs
├── Aggregates/
│   ├── DeviceAggregate.cs
├── Services/
│   ├── ReadingValidationService.cs
│   ├── AlertService.cs
├── Events/
│   ├── ReadingReceivedEvent.cs
│   ├── HeartbeatReceivedEvent.cs
│   ├── BatteryChangeNotifiedEvent.cs
├── Repositories/
│   ├── IDeviceRepository.cs
│   ├── ITemperatureReadingRepository.cs
│   ├── IHeartbeatRepository.cs
│   ├── IBatteryChangeRepository.cs
```

## Implementation Details
### Entities
- **Temperature reading:** Represents a temperature reading from a device, including properties like `DeviceId`, `CreatedAt`, `SignalStrenght` (inherited from abstract Reading) and `Temperature`.

- **Heartbeat:** Represents a heartbeat signal from a device, including properties like `DeviceId`, `CreatedAt`.

- **BatteryChange:** Represents a battery change notification, including properties like `DeviceId`, `CreatedAt`.

- **Device:** Represents a device, including properties like `DeviceId`, `Name`, and `Status`.

### Aggregates
- **DeviceAggregate:** Groups related entities and provides methods for managing readings, heartbeats, and battery changes.

### Domain Services
- **ReadingValidationService:** Validates readings for duplicates and ensures idempotency.
- **AlertService:** Generates alerts if the latest temperature reading is older than 30 minutes.

### Events
- **ReadingReceivedEvent:** Triggered when a new reading is received.
- **HeartbeatReceivedEvent:** Triggered when a new heartbeat is received.
- **BatteryChangeNotifiedEvent:** Triggered when a battery change notification is received.

### Repositories
- **IBatteryChangeRepository:** Interface for managing battery change data.
- **IDeviceRepository:** Interface for managing device data.
- **IHeartbeatRepository:** Interface for managing heartbeat data.
- **ITemperatureReadingRepository:** Interface for managing reading data.

## Conclusion
The proposed domain layer structure ensures a clean, scalable, and testable architecture for the Beach Weather Station system. By adhering to clean architecture principles, the domain layer will provide a solid foundation for the application and infrastructure layers.
