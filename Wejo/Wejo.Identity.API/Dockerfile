# Base stage: Runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# Run as non-root user for security
RUN addgroup --system appgroup && adduser --system --ingroup appgroup app
USER app

# Build stage: Compile and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy .csproj files first for caching
COPY ["Wejo.Identity.API/Wejo.Identity.API/Wejo.Identity.API.csproj", "Wejo.Identity.API/"]
COPY ["Wejo.Common.Core/Wejo.Common.Core.csproj", "Wejo.Common.Core/"]
COPY ["Wejo.Common.SeedWork/Wejo.Common.SeedWork.csproj", "Wejo.Common.SeedWork/"]
COPY ["Wejo.Identity.Application/Wejo.Identity.Application.csproj", "Wejo.Identity.Application/"]
COPY ["Wejo.Common.Domain/Wejo.Common.Domain.csproj", "Wejo.Common.Domain/"]
RUN dotnet restore "./Wejo.Identity.API/Wejo.Identity.API.csproj"
COPY . .
WORKDIR "/src/Wejo.Identity.API"
# Publish the app to /app/publish
RUN dotnet publish "./Wejo.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: Production-ready image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
# Run the app
ENTRYPOINT ["dotnet", "Wejo.Identity.API.dll"]