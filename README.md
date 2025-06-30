## ✨ Features
- 🔁 Live currency conversion using CurrencyLayer API
- 🧠 Domain-centric design following Hexagonal Architecture (Ports & Adapters)
- 📦 Generic MongoDB repository implementation
- 🗃️ Soft delete functionality
- 🚀 Refit clients with Polly resilience policies
- 🧪 Integration tests using Testcontainers
- 🧰 Clean separation of concerns (Domain, App, Infra, API)

## 🧱 Architecture
This service follows the **Hexagonal Architecture (Clean Architecture)** pattern:
- **Domain Layer**: Pure business logic, no framework dependencies.
- **Application Layer**: Use case coordination, service interfaces.
- **Infrastructure Layer**: MongoDB, HTTP clients, external services.
- **API Layer**: Entry point (Web API) exposing endpoints.

### Highlights:
- ✅ Strong separation of concerns
- ✅ Mongo repository follows a generic, reusable pattern
- ✅ Domain models remain persistence-agnostic
- ✅ Infrastructure uses Mongo-specific DTOs, mapped to/from domain

## 🧰 Tech Stack

- **.NET 8 / C#**
- **MongoDB** (via Docker)
- **Refit** (typed HTTP clients)
- **Polly** (resilience policies)
- **Testcontainers** (integration testing)
- **xUnit + FakeItEasy** (unit testing)
- **Docker** (for local Mongo)
