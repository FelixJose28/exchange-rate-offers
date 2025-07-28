# Imagen base de Red Hat UBI con .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa de construcción (Build) -  compiles the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar solo el .csproj de la API para restaurar dependencias
COPY ["src/ExchangeRateOffers.Api/ExchangeRateOffers.Api.csproj", "ExchangeRateOffers.Api/"]

# Temporalmente root para restaurar paquetes
USER root  
RUN dotnet restore "ExchangeRateOffers.Api/ExchangeRateOffers.Api.csproj"
COPY ./src  .

WORKDIR "/src/ExchangeRateOffers.Api"
# Compilar el proyecto API
RUN dotnet build "ExchangeRateOffers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build /p:UseAppHost=false

# Asegurar permisos antes de cambiar de usuario
RUN chown -R 1001:0 /app

# Etapa de publicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ExchangeRateOffers.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final: imagen liviana con solo el binario compilado
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER 1001
ENTRYPOINT ["dotnet", "ExchangeRateOffers.Api.dll"]