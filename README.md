# Beach Weather Station

**Beach Weather Station** is an end-to-end IoT solution for monitoring and analyzing ocean water temperature at the beach. The system consists of a marine-grade temperature-sensing buoy, an indoor gateway for data collection, a cloud-hosted Web API with AI/LLM query support, and a public iOS app for real-time data visualization and chatbot interaction.

## Components

- **Buoy (IoT Device):**  
  A waterproof, battery-powered buoy equipped with an Arduino MKRWAN and DS18B20 temperature sensor. It collects water temperature data every 30 minutes and transmits readings via LoRaWAN.

- **Gateway:**  
  An indoor, mains-powered device (Arduino MKRWAN with Ethernet Shield) that receives LoRaWAN messages from the buoy, buffers and forwards them to the cloud API over HTTP.

- **Web API & Database:**  
  Hosted on Azure Functions (.NET 9, C#), the API ingests temperature data, stores it in Cosmos DB via Entity Framework, and exposes endpoints for historical and real-time data. It also supports AI/LLM-powered natural language queries for advanced insights.

- **iOS App:**  
  A Swift/SwiftUI app for the public, displaying current and historical temperatures from the buoy. The app includes both traditional UI and an AI chatbot interface for conversational data exploration.

## Features

- **Reliable Marine Sensing:** Robust hardware and firmware for accurate, long-term temperature monitoring in ocean environments.
- **Seamless Data Flow:** Secure, resilient data transmission from buoy to gateway to cloud.
- **Scalable Cloud Backend:** Modern serverless architecture, easily extensible to more buoys or new sensor types.
- **AI-Powered Insights:** Natural language queries enabled via integration with Azure OpenAI or similar LLMs.
- **User-Friendly App:** Intuitive, accessible iOS app with both chart-based and chatbot-driven data interaction.

---

*For setup instructions, technical architecture, and contributing guidelines, see the respective documentation in each component’s directory.*