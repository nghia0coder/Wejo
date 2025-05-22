# Wejo - Sports Friend Connection Platform 🏀⚽

<img src="https://github.com/user-attachments/assets/c15c1167-d7f8-414b-8cea-02bc02328c90" alt="Wejo Logo" width="100"/>

[![.NET Core](https://img.shields.io/badge/.NET-Core-blueviolet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-2496ED)](https://www.docker.com/)
[![Cassandra](https://img.shields.io/badge/Cassandra-1287B1)](https://cassandra.apache.org/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600)](https://www.rabbitmq.com/)
[![Jenkins](https://img.shields.io/badge/Jenkins-D24939)](https://www.jenkins.io/)

## 🚀 Project Overview

Wejo is a **microservices-based sports friend connection platform** designed to unite sports enthusiasts in real-time. Users can create accounts, discover sports partners, join activities, chat with other athletes, and receive instant notifications. Built with **.NET Core** and adhering to **Domain-Driven Design (DDD)** principles, Wejo ensures scalability, performance, and a seamless user experience.

---

### ⚠️ Note

######  Please note that sensitive information such as IP addresses, database configurations, and other credentials in this project are currently set for the **testing environment** only. These are included for convenience during development and testing. For production deployment, these values will be replaced with secure and appropriate configurations using environment variables or other secure methods.


## 🏗️ Project Structure

Wejo is organized into modular microservices, each serving a distinct purpose:

### Core Services

| Service                  | Description                                                                 |
|--------------------------|-----------------------------------------------------------------------------|
| 🔐 **Identity Service**  | Manages user authentication, registration, profile, and social login.       |
| 🏃 **Activity Service**  | Handles sports activity creation, searching, and participant management.    |
| ⚡️ **Realtime Service** | Enables real-time communication via SignalR for chat and notifications.     |
| 📬 **Notification Service** | Manages user notifications and messaging.                                 |
| ⏰ **Background Job Service** | Executes scheduled and background processing tasks.                     |

### Shared Components

- 📚 **Common.Core**: Shared utilities, base controllers, and common functionality.
- 🗂️ **Common.Domain**: Shared domain models and database context.
- 🛠️ **Common.SeedWork**: Foundational components for the domain model.

### Architecture Layers (per service)

- 🌐 **API**: Presentation layer with controllers handling HTTP requests.
- 💡 **Application**: Application logic with CQRS command/query handlers.
- 🛢️ **Infrastructure**: Database access, external services, and integrations.

---

## 🛠️ Technology Stack

Wejo leverages a modern, robust tech stack to deliver a high-performance platform:

| Component                | Technology                |
|-------------------------|---------------------------|
| **Backend**             | .NET Core, ASP.NET Core  |
| **Real-time Communication** | SignalR                |
| **Database**            | PostgreSQL, Cassandra    |
| **Messaging**           | RabbitMQ                |
| **Caching**             | Redis                   |
| **Containerization**    | Docker                  |
| **CI/CD**               | Jenkins                 |

---

## 🖥️ Infrastructure Components

| Component     | Role                                                  | Icon |
|---------------|-------------------------------------------------------|------|
| **PostgreSQL** | Relational database for user data and activities      | 🗄️   |
| **Cassandra** | NoSQL database for high-performance data storage      | 📊   |
| **RabbitMQ**  | Message broker for asynchronous service communication | 🐰   |
| **Redis**     | In-memory caching for session and performance         | ⚡   |

---

---
## ☁️ Deployment Environment


Wejo leverages Google Cloud for production-grade scalability and reliability, while a dedicated Virtual Private Server (VPS) serves as the development environment to streamline team collaboration. All infrastructure components, including databases, CI/CD tools, and caching, are containerized using Docker to ensure consistency and ease of setup for developers.

Development: VPS
Setup: A rented VPS (e.g., DigitalOcean, Linode, or AWS EC2) hosts the development environment.
Dockerized Components:

🗄️ PostgreSQL: Relational database for user and activity data.

📊 Cassandra: NoSQL database for chat messages and read statuses.

🐰 RabbitMQ: Message broker for async communication.

⚡ Redis: In-memory cache for user info and session management.

🛠️ Jenkins: CI/CD server for automated builds and deployments.


Configuration:

- All components run as Docker containers, defined in docker-compose.yml.

- VPS specs: Recommended 8GB RAM, 4 vCPUs, 100GB SSD to handle multiple containers.

- Exposed ports (e.g., 5432 for PostgreSQL, 9042 for Cassandra, 8080 for Jenkins) are secured with firewall rules and VPN access for team members.

🤝 Team Development Benefits


- Consistency: Docker ensures identical environments across team members’ machines and the VPS, eliminating "it works on my machine" issues.

- Ease of Setup: Developers clone the repository, pull the docker-compose.yml, and run docker-compose up -d to start all services.

- Centralized Access: The VPS provides a shared environment for testing and debugging, accessible via SSH or VPN.

- CI/CD Integration: Jenkins on the VPS automates builds, tests, and deployments, streamlining workflows.

---

## 🚀 Deployment

Wejo uses **Docker** and **Docker Compose** for streamlined development and deployment:

```bash
# Build and run all services
docker-compose up -d

# Build a specific service
docker-compose build game-service

# Build and run a specific service
docker-compose up -d --force-recreate game-service
```

> **Tip**: Use `docker-compose logs` to monitor service logs in real-time! 📜

---

## 📝 Development Guidelines

### 🛠️ Creating a New Service

1. **Create Dockerfile**:
   - Add a `Dockerfile` in the service's root directory, following the pattern of existing services.
2. **Update `docker-compose.yml`**:
   - Add the new service with ports, environment variables, and dependencies.
3. **Update Jenkinsfile**:
   - Add Docker image variable in the `environment` section.
   - Update the **Detect Changes** stage to check for service changes.
   - Add checks in **Build & Push** and **Deploy** stages for the new service.

### 🗄️ Database Scaffolding

Scaffold the PostgreSQL database context using Entity Framework:

```bash
dotnet ef dbcontext scaffold "Host={Host};Database={DbName};Username={UserName};Password={Password}" Npgsql.EntityFrameworkCore.PostgreSQL -o Database -c WejoContext --project .\Wejo.Common.Domain --force
```

> **Note**: Ignore the "Could not load database collations" warning—it’s harmless! 😊

### 🐳 Building and Running Docker Images Manually

```bash
# Build image
docker build -t wejo_realtime_service -f Wejo.Realtime.API/Dockerfile .

# Run image
docker run -d -p 5000:80 --name wejo_realtime_service wejo_realtime_service
```

### 📡 Connecting to Cassandra

Access the Cassandra container for database operations:

```bash
docker exec -it cassandra cqlsh
```

---

## 🔄 CI/CD Pipeline

Wejo uses **Jenkins** for automated CI/CD, ensuring fast and reliable deployments:

1. **Checkout** 📥: Pulls the latest code from the repository.
2. **Detect Changes** 🔍: Identifies modified services.
3. **Build & Push** 🛠️: Builds and pushes Docker images for changed services.
4. **Deploy** 🚀: Updates running services with new versions.

> **Efficiency**: Only modified services are rebuilt and redeployed, unless common modules are updated, triggering a full rebuild.
