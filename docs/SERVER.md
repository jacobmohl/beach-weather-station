# Web API & Database

## Overview

This document outlines the technical specifications and requirements for the Beach Weather Station Web API solution.

## Platform & Tools
- **API:** Azure Functions (C#)
- **Database:** Cosmos DB
- **Architecture:** Clean Architecture without CRQS, but influences by vertical sliging.

## Functional Requirements
- Receive temperature readings from gateway via authenticated HTTP POST
  - Get the latest temperature reading.
  - Get the temperature readings for the latest 24 hour, including the higest and the lowest reading.
  - Get the average temperature readings (average, lowest, highest) pr. day for the latest 30 days.
- Validate and store readings (device ID, timestamp, value)
- Prevent duplicate records (idempotency)
- Endpoints for:
  - Ingesting new readings
  - Ingesting heartbeats from gateway
  - Ingesting battery change notifications from app/management
  - Retrieving latest reading, heatbeats and battery changes
  - Retrieving historical data (by time range)
  - System health check
- Alerts if readings are delayed (no readings for 30 minutes)
- **AI Query Support:** 
  - Natural language query endpoint powered by LLM
  - Allow users/app to submit questions about data (e.g., trends, stats) and get AI-generated answers

## Non-Functional Requirements
- Scalable to support frequent readings
- High reliability, uptime, and data durability
- Low-latency API responses for real-time display
- Secure (HTTPS, authentication for gateway)
- Comprehensive logging and error reporting
- Use file-scoped namespaces
- Use Entity Framework to access CosmosDB through the Repository pattern