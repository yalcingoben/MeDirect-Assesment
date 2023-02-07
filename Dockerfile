#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/ExchangeTrader.Api/ExchangeTrader.Api.csproj", "src/Presentation/ExchangeTrader.Api/"]
COPY ["src/Core/ExchangeTrader.App/ExchangeTrader.App.csproj", "src/Core/ExchangeTrader.App/"]
COPY ["src/Core/ExchangeTrader.App.Abstractions/ExchangeTrader.App.Abstractions.csproj", "src/Core/ExchangeTrader.App.Abstractions/"]
COPY ["src/Infrastructure/ExchangeTrader.Caching.Redis/ExchangeTrader.Caching.Redis.csproj", "src/Infrastructure/ExchangeTrader.Caching.Redis/"]
COPY ["src/Infrastructure/ExchangeTrader.Integration.ExchangeRatesApi/ExchangeTrader.Integration.ExchangeRatesApi.csproj", "src/Infrastructure/ExchangeTrader.Integration.ExchangeRatesApi/"]
COPY ["src/Infrastructure/ExchangeTrader.Integration.Fixer/ExchangeTrader.Integration.Fixer.csproj", "src/Infrastructure/ExchangeTrader.Integration.Fixer/"]
RUN dotnet restore "src/Presentation/ExchangeTrader.Api/ExchangeTrader.Api.csproj"
COPY . .
WORKDIR "/src/src/Presentation/ExchangeTrader.Api"
RUN dotnet build "ExchangeTrader.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExchangeTrader.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExchangeTrader.Api.dll"]