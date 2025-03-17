# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

ARG ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=$ENVIRONMENT

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wejo.Backgroud.Job/Wejo.Background.Job.csproj", "Wejo.Backgroud.Job/"]
COPY ["Wejo.Common.Core/Wejo.Common.Core.csproj", "Wejo.Common.Core/"]
COPY ["Wejo.Common.SeedWork/Wejo.Common.SeedWork.csproj", "Wejo.Common.SeedWork/"]
COPY ["Wejo.Common.Domain/Wejo.Common.Domain.csproj", "Wejo.Common.Domain/"]
RUN dotnet restore "./Wejo.Backgroud.Job/Wejo.Background.Job.csproj"
COPY . .
WORKDIR "/src/Wejo.Backgroud.Job"
RUN dotnet build "./Wejo.Background.Job.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wejo.Background.Job.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

COPY Wejo.Backgroud.Job/appsettings.json /app/appsettings.json

ENTRYPOINT ["dotnet", "Wejo.Background.Job.dll"]
