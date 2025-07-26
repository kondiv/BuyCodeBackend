FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BuyCodeBackend.Auth/BuyCodeBackend.Auth.csproj", "BuyCodeBackend.Auth/"]
RUN dotnet restore "BuyCodeBackend.Auth/BuyCodeBackend.Auth.csproj"
COPY . .
WORKDIR "/src/BuyCodeBackend.Auth"
RUN dotnet build "./BuyCodeBackend.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BuyCodeBackend.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BuyCodeBackend.Auth.dll"]
