# Beach Weather Station — Repository Structure Proposal

This proposal outlines a recommended repository structure for the Beach Weather Station project, supporting the development and maintenance of all system components: Buoy (IoT), Gateway, Web API (Azure Functions), and iOS App.

---

## 1. Monorepo vs. Polyrepo

Given the tightly coupled nature of hardware and software components, a **monorepo** approach is recommended:
- Single repository with clear subfolders for each subsystem.
- Simplifies cross-component integration, versioning, and CI/CD pipelines.
- Easy for new contributors to understand the whole system.

---

## 2. Proposed Directory Layout

```
beach-weather-station/
│
├── README.md
├── docs/
│   └── architecture.md
│   └── maintenance.md
│   └── hardware-schematics/
│
├── buoy-firmware/
│   ├── src/
│   ├── include/
│   ├── lib/
│   ├── platformio.ini / arduino_project.ino
│   └── README.md
│
├── gateway-firmware/
│   ├── src/
│   ├── include/
│   ├── lib/
│   ├── platformio.ini / arduino_project.ino
│   └── README.md
│
├── web-api/
│   ├── src/
│   │   └── Functions/
│   ├── tests/
│   ├── WebApi.sln
│   ├── WebApi.csproj
│   └── README.md
│
├── ios-app/
│   ├── BeachWeatherApp/
│   ├── BeachWeatherApp.xcodeproj
│   ├── Tests/
│   └── README.md
│
├── shared/
│   ├── protocol-specs/
│   ├── api-contracts/
│   └── README.md
│
├── tools/
│   └── deployment-scripts/
│
├── .github/
│   ├── workflows/
│   └── ISSUE_TEMPLATE/
│
└── LICENSE
```

---

## 3. Component Breakdown

### /buoy-firmware/
- Source code for Arduino-based buoy firmware.
- Sensor drivers, LoRaWAN transmission logic, power management.

### /gateway-firmware/
- Source code for Arduino-based gateway firmware.
- LoRaWAN reception, Ethernet connectivity, buffering, API communication.

### /web-api/
- Azure Functions (C#) project.
- API endpoints for data ingestion, retrieval, AI (LLM) queries.
- Cosmos DB integration.

### /ios-app/
- Swift/SwiftUI iOS application.
- Traditional and AI chatbot UI for temperature readings and trends.

### /shared/
- Protocol definitions (LoRaWAN payloads, API schemas).
- Shared data models and contracts.

### /docs/
- High-level architecture, setup guides, hardware diagrams.

### /tools/
- CI/CD, deployment, and utility scripts.

---

## 4. CI/CD & Automation

- Place GitHub Actions workflows in `.github/workflows/` for:
  - Linting, building, and testing all components.
  - Deploying firmware images, API releases, and iOS betas.

---

## 5. Notes

- Consider using submodules or separate repos if any component grows significantly in complexity.
- Each component should have its own `README.md` with build instructions.
- Maintain clear versioning and changelogs per component.

---

**This structure ensures modularity, clarity, and ease of cross-team development for the Beach Weather Station project.**