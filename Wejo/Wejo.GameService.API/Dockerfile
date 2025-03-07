# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#Game
# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wejo.Game.API/Wejo.Game.API.csproj", "Wejo.Game.API/"]
COPY ["Wejo.Common.Core/Wejo.Common.Core.csproj", "Wejo.Common.Core/"]
COPY ["Wejo.Common.SeedWork/Wejo.Common.SeedWork.csproj", "Wejo.Common.SeedWork/"]
COPY ["Wejo.Game.Application/Wejo.Game.Application.csproj", "Wejo.Game.Application/"]
COPY ["Wejo.Common.Domain/Wejo.Common.Domain.csproj", "Wejo.Common.Domain/"]
RUN dotnet restore "./Wejo.Game.API/Wejo.Game.API.csproj"
COPY . .
WORKDIR "/src/Wejo.Game.API"
RUN dotnet build "./Wejo.Game.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wejo.Game.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Wejo.Game.API/appsettings.json /app/appsettings.json

ENTRYPOINT ["dotnet", "Wejo.Game.API.dll"]