# Web API & Database

## Overview

This document outlines the technical specifications and requirements for the Beach Weather Station Web API solution.

## Platform & Tools
- **API:** Azure Functions (C#)
- **Database:** Cosmos DB
- **Architecture:** Clean Architecture without CRQS, but influences by vertical sliging.

## Functional Requirements
- Receive readings from gateway via authenticated HTTP POST
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