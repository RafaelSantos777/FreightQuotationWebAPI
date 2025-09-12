FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CargofiveService/CargofiveService.csproj", "CargofiveService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "CargofiveService/CargofiveService.csproj"
COPY . .
WORKDIR "/src/CargofiveService"
RUN dotnet build "./CargofiveService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CargofiveService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CargofiveService.dll"]
