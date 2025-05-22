# Wejo - Sports Friend Connection Platform

## Project Overview

Wejo is a microservices-based sports friend connection platform designed to connect sports enthusiasts in real-time. The platform allows users to create accounts, discover sports partners, join activities, chat with other athletes, and receive real-time notifications. The system is built using .NET Core and follows a clean architecture approach with Domain-Driven Design (DDD) principles.

## Project Structure

The solution is organized into several microservices:

### Core Services

- **Identity Service** (`Wejo.Identity.API`): Manages user authentication, registration, profile management, and social login features.
- **Activity Service** (`Wejo.Game.API`): Handles sports activity creation, searching, and management of participants.
- **Realtime Service** (`Wejo.Realtime.API`): Provides real-time communication capabilities through SignalR hubs for chat and notifications.
- **Notification Service** (`Wejo.Notification.API`): Manages user notifications and messaging.
- **Background Job Service** (`Wejo.Background.Job`): Handles scheduled and background processing tasks.

### Shared Components

- **Common.Core**: Contains shared utilities, base controllers, and common functionality.
- **Common.Domain**: Contains shared domain models and database context.
- **Common.SeedWork**: Provides foundational components for the domain model.

### Architecture Layers (per service)

- **API**: The presentation layer with controllers handling HTTP requests.
- **Application**: Contains application logic, command/query handlers (CQRS pattern).
- **Infrastructure**: Manages external concerns like database access, external services, etc.

## Technology Stack

- **Backend**: .NET Core, ASP.NET Core
- **Real-time Communication**: SignalR
- **Database**: PostgreSQL, Cassandra
- **Messaging**: RabbitMQ
- **Caching**: Redis
- **Containerization**: Docker
- **CI/CD**: Jenkins

## Infrastructure Components

- **PostgreSQL**: Main relational database for storing user data, activity information, etc.
- **Cassandra**: NoSQL database for certain data types that benefit from its distributed nature.
- **RabbitMQ**: Message broker for asynchronous communication between services.
- **Redis**: In-memory data store used for caching and session management.

## Deployment

The project uses Docker containers orchestrated with Docker Compose for development and deployment:

```bash
# Build and run all services
docker-compose up -d

# Build a specific service
docker-compose build game-service

# Build and run a specific service
docker-compose up -d --force-recreate game-service
```

## Development Guidelines

### Creating a New Service

1. Create a Dockerfile for your service
2. Add the service to docker-compose.yml following the existing pattern
3. Update Jenkinsfile:
   - Add a new Docker image variable name in environment
   - Add environment variable in the "Detect Changes" stage
   - Add if check in "Build & Push Updated Service" stage
   - Add if check in "Deploy Updated Service" stage

### Database Scaffolding

```bash
dotnet ef dbcontext scaffold "Host={Host};Database={DbName};Username={UserName};Password={Password}" Npgsql.EntityFrameworkCore.PostgreSQL -o Database -c WejoContext --project .\Wejo.Common.Domain --force
```

Note: You may see a "Could not load database collations" message - this can be safely ignored.

### Building and Running Docker Images Manually

```bash
# Build image example
docker build -t wejo_realtime_service -f Wejo.Realtime.API/Dockerfile .

# Run image example
docker run -d -p 5000:80 --name wejo_realtime_service wejo_realtime_service
```

### Connecting to Cassandra

```bash
docker exec -it cassandra cqlsh
```

## CI/CD Pipeline

The project uses Jenkins for continuous integration and deployment:

1. **Checkout**: Pulls the code from the repository
2. **Detect Changes**: Determines which services have been modified
3. **Build & Push**: Builds and pushes Docker images for modified services
4. **Deploy**: Updates the running services with the new versions

The pipeline is designed to only rebuild and redeploy services that have been modified, unless changes are made to common modules, in which case all services are rebuilt.
