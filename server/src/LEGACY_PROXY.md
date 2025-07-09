# Legacy API Proxy

## Overview

The `LegacyProxyFunction` provides backward compatibility with the old legacy API schemas by acting as a proxy that converts legacy formats to the new standardized format.

## Endpoints

### Legacy Readings (Unified Endpoint)

**POST** `/api/readings`

Accepts legacy temperature readings, heartbeats, and sensor errors at a single endpoint. The function automatically detects the payload type based on the `sensorType` field and routes accordingly.

#### Temperature Readings

**Legacy Input Schema:**
```json
{
   "sensorType":"WaterTemperature",
   "unit": "C",
   "reading":"18.02",
   "sensorId": "Sensor1",
   "signalStrength": "82"
}
```

**Converted to New Schema:**
```json
{
   "deviceId": "auto-generated-guid",
   "createdAt": "2025-07-09T10:00:00Z",
   "temperature": 18.02,
   "signalStrength": 82
}
```

#### Heartbeats

**Legacy Input Schema:**
```json
{
  "sensorType":"Heatbeat",
  "unit":"Count",
  "reading":1,
  "sensorId":"Sensor1"
}
```

**Converted to New Schema:**
```json
{
   "deviceId": "auto-generated-guid",
   "createdAt": "2025-07-09T10:00:00Z"
}
```

#### Sensor Errors

**Legacy Input Schema:**
```json
{
  "id": "78470efa-91ae-450e-9c3e-55119cfd0240",
  "sensorType": "SensorError",
  "unit": "Error",
  "reading": 0,
  "sensorId": "Sensor1",
  "capturedAt": "2025-07-08T20:37:01.5019778Z",
  "meta": {
    "json": "{\"corrupted sensor data...\"}"
  },
  "signalStrength": null,
  "timeToLive": 604800
}
```

**Processing:**
- Logs detailed error information including corrupted data from meta.json
- Triggers alert notifications using the AlertService
- Returns success acknowledgment to legacy system

## Device Mapping

The proxy function automatically maps legacy `sensorId` values to device GUIDs:

1. **Existing Devices**: If a device with the same name as the `sensorId` already exists, its GUID is used.
2. **New Devices**: If no matching device exists, a new device is created with:
   - A deterministic GUID generated from the `sensorId`
   - Name set to the `sensorId` value
   - Status set to "Online"

## Error Handling

The proxy function validates incoming data and returns appropriate HTTP status codes:

- **400 Bad Request**: Invalid request format or missing required fields
- **500 Internal Server Error**: Unexpected errors during processing

## Implementation Details

- **Single Endpoint Architecture**: Uses one endpoint that routes based on the `sensorType` field for simplified integration
- **Automatic Payload Detection**: Parses JSON to detect whether the payload is a temperature reading (`WaterTemperature`), heartbeat (`Heatbeat`), or sensor error (`SensorError`)
- **Deterministic GUID Generation**: Uses SHA-1 hashing with a namespace GUID to ensure the same `sensorId` always maps to the same device GUID
- **Concurrent Safety**: Handles race conditions when multiple requests try to create the same device simultaneously
- **Logging**: Comprehensive logging for debugging and monitoring with specific context for each payload type
- **Error Alerting**: Integrates with AlertService to notify about sensor errors and corrupted data
- **Type Conversion**: Safely converts string values to appropriate numeric types with fallback defaults
- **Case-Insensitive JSON**: Accepts JSON with different casing for property names

## Testing

Use the provided `LegacyAPI.http` file to test the endpoints with sample data.
