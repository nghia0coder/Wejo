# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#Notification
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8084

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wejo.Notification.API/Wejo.Notification.API.csproj", "Wejo.Notification.API/"]
COPY ["Wejo.Common.Core/Wejo.Common.Core.csproj", "Wejo.Common.Core/"]
COPY ["Wejo.Common.SeedWork/Wejo.Common.SeedWork.csproj", "Wejo.Common.SeedWork/"]
COPY ["Wejo.Notification.Application/Wejo.Notification.Application.csproj", "Wejo.Notification.Application/"]
COPY ["Wejo.Common.Domain/Wejo.Common.Domain.csproj", "Wejo.Common.Domain/"]
RUN dotnet restore "./Wejo.Notification.API/Wejo.Notification.API.csproj"
COPY . .
WORKDIR "/src/Wejo.Notification.API"
RUN dotnet build "./Wejo.Notification.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wejo.Notification.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Wejo.Notification.API/appsettings.json /app/appsettings.json

ENTRYPOINT ["dotnet", "Wejo.Notification.API.dll"]