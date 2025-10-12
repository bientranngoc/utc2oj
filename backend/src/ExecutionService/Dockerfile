FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExecutionService/ExecutionService.csproj", "ExecutionService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "ExecutionService/ExecutionService.csproj"
COPY . .
WORKDIR "/src/ExecutionService"
RUN dotnet build "ExecutionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExecutionService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExecutionService.dll"]