# ------------------- STAGE 1: BUILD -------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["src/ExchangeRateOffers.Api/ExchangeRateOffers.Api.csproj", "src/ExchangeRateOffers.Api/"]

# Restaurar dependencias
RUN dotnet restore "src/ExchangeRateOffers.Api/ExchangeRateOffers.Api.csproj"

# Copiar el resto del código
COPY ./src ./src

# Publicar en modo Release
RUN dotnet publish "src/ExchangeRateOffers.Api/ExchangeRateOffers.Api.csproj" -c Release -o /app/publish

# ------------------- STAGE 2: RUNTIME -------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "ExchangeRateOffers.Api.dll"]
