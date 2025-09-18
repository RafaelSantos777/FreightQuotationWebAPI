#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["QuotationService/QuotationService.csproj", "QuotationService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "QuotationService/QuotationService.csproj"
COPY . .

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR "/src/QuotationService"
RUN dotnet build "./QuotationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuotationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src /src

COPY ./QuotationService/entrypoint.sh .

ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["/bin/bash", "./entrypoint.sh"]
CMD ["dotnet", "QuotationService.dll"]

