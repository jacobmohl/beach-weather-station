# Beach Weather Station — Technical Specifications & Requirements

## Overview

This document outlines the technical specifications and requirements for the Beach Weather Station solution, which consists of four main components:

- **Buoy (IoT Device)**
- **Gateway**
- **Web API & Database**
- **iOS App**

---

## 1. Buoy (IoT Device)

### Hardware
- **Microcontroller:** Arduino MKRWAN
- **Temperature Sensor:** DS18B20 (waterproof)
- **Power:** Battery operated, optimized for long life
- **Enclosure:** Waterproof, corrosion-resistant, suitable for marine environments

### Firmware & Functional Requirements
- Sample water temperature every 30 minutes
- Enter low-power mode between measurements
- Each reading includes:
  - Unique device ID
  - Timestamp (synchronized via gateway or local RTC)
  - Temperature value (°C)
- Transmit readings to gateway via LoRaWAN
- Handle and report transmission or sensor errors
- Diagnostic indicator (e.g., LED) for maintenance

### Non-Functional Requirements
- Battery life must support at least several weeks between maintenance intervals
- Physical design must withstand saltwater, sun, and temperature extremes
- Should be easily retrievable and serviceable

---

## 2. Gateway

### Hardware
- **Microcontroller:** Arduino MKRWAN with Ethernet Shield
- **Location:** Indoor (no weatherproofing required)
- **Power:** Stable mains connection

### Software & Functional Requirements
- Receive LoRaWAN messages from buoy
- Parse and validate data (check integrity, parse ID, timestamp, value)
- Forward data to central Web API via HTTP (Ethernet)
- Buffer readings locally if network/API is unavailable, and retry transmission
- Log gateway activity and errors for diagnostics
- Configurable settings (API endpoint, retry intervals) via local interface or OTA update
- Watchdog/reset functionality for resilience

### Non-Functional Requirements
- Must handle connectivity interruptions gracefully
- Must support firmware or OTA updates

---

## 3. Web API & Database

### Platform & Tools
- **API:** Azure Functions (C#)
- **Database:** Cosmos DB

### Functional Requirements
- Receive readings from gateway via authenticated HTTP POST
- Validate and store readings (device ID, timestamp, value)
- Prevent duplicate records (idempotency)
- Endpoints for:
  - Ingesting new readings
  - Retrieving latest reading
  - Retrieving historical data (by time range)
  - System health check
- **AI Query Support:** 
  - Natural language query endpoint powered by LLM
  - Allow users/app to submit questions about data (e.g., trends, stats) and get AI-generated answers

### Non-Functional Requirements
- Scalable to support multiple buoys and frequent readings
- High reliability, uptime, and data durability
- Low-latency API responses for real-time display
- Secure (HTTPS, authentication for gateway)
- Compliance with regional data privacy regulations
- Comprehensive logging and error reporting

---

## 4. iOS App

### Platform & Tools
- **Language:** Swift
- **Framework:** SwiftUI

### Functional Requirements
- Publicly available (no user authentication required)
- Connects to Web API to:
  - Display current water temperature (for a single buoy)
  - Show historical trends (graphs, charts)
  - Manual and automatic data refresh
  - Handle offline mode (cache last readings)
- **AI Chatbot Interface:**
  - Embedded chatbot for natural language queries (e.g., “What’s the trend this week?”)
  - Traditional UI for reading display and history navigation

### Non-Functional Requirements
- Responsive, user-friendly, and visually clear
- Minimal battery and data consumption
- Secure handling of API keys and network communications
- Accessibility support (VoiceOver, large text)
- Localization support (optional)

---

## 5. General System Requirements

- Modular design to allow for future expansion (additional buoys, gateways)
- Encrypted data transmission (LoRaWAN and HTTPS)
- Easy deployment and maintenance procedures
- Documentation for hardware setup, software deployment, and API usage
- Compliance with local radio transmission and data privacy laws
- (Optional) Remote diagnostics dashboard for system admins

---

## Appendix

- [ ] Detailed hardware schematics
- [ ] API specification and documentation
- [ ] Maintenance schedules and procedures
- [ ] Troubleshooting and diagnostics guides