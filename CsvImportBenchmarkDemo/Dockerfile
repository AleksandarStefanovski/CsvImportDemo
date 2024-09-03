FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CsvImportBenchmarkDemo/CsvImportBenchmarkDemo.csproj", "CsvImportBenchmarkDemo/"]
RUN dotnet restore "./CsvImportBenchmarkDemo/CsvImportBenchmarkDemo.csproj"
COPY . .
WORKDIR "/src/CsvImportBenchmarkDemo"
RUN dotnet build "./CsvImportBenchmarkDemo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CsvImportBenchmarkDemo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CsvImportBenchmarkDemo.dll"]
