# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wejo.Identity.API/Wejo.Identity.API.csproj", "Wejo.Identity.API/"]
COPY ["Wejo.Common.Core/Wejo.Common.Core.csproj", "Wejo.Common.Core/"]
COPY ["Wejo.Common.SeedWork/Wejo.Common.SeedWork.csproj", "Wejo.Common.SeedWork/"]
COPY ["Wejo.Identity.Application/Wejo.Identity.Application.csproj", "Wejo.Identity.Application/"]
COPY ["Wejo.Common.Domain/Wejo.Common.Domain.csproj", "Wejo.Common.Domain/"]
RUN dotnet restore "./Wejo.Identity.API/Wejo.Identity.API.csproj"
COPY . .
WORKDIR "/src/Wejo.Identity.API"
RUN dotnet build "./Wejo.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wejo.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Wejo.Identity.API/appsettings.json /app/appsettings.json
COPY Wejo.Identity.API/appsettings.$ENVIRONMENT.json /app/appsettings.$ENVIRONMENT.json

ENTRYPOINT ["dotnet", "Wejo.Identity.API.dll"]