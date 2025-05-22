FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8083
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wejo.Realtime.API/Wejo.Realtime.API.csproj", "Wejo.Realtime.API/"]
COPY ["Wejo.Common.Core/Wejo.Common.Core.csproj", "Wejo.Common.Core/"]
COPY ["Wejo.Common.SeedWork/Wejo.Common.SeedWork.csproj", "Wejo.Common.SeedWork/"]
COPY ["Wejo.Common.Domain/Wejo.Common.Domain.csproj", "Wejo.Common.Domain/"]
RUN dotnet restore "./Wejo.Realtime.API/Wejo.Realtime.API.csproj"
COPY . .
WORKDIR "/src/Wejo.Realtime.API"
RUN dotnet build "./Wejo.Realtime.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wejo.Realtime.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Wejo.Realtime.API/appsettings.json /app/appsettings.json

ENV ASPNETCORE_URLS=http://+:8083;http://+:5001

ENTRYPOINT ["dotnet", "Wejo.Realtime.API.dll"]
