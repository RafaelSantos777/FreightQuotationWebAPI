FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserService/UserService.csproj", "UserService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "UserService/UserService.csproj"
COPY . .
RUN dotnet tool install --global dotnet-ef
WORKDIR "/src/UserService"
RUN dotnet build "./UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"
COPY ./UserService/entrypoint.sh .

ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["/bin/bash", "./entrypoint.sh"]
CMD ["dotnet", "UserService.dll"]
