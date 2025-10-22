FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ContestService/ContestService.csproj", "ContestService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "ContestService/ContestService.csproj"
COPY . .
WORKDIR "/src/ContestService"
RUN dotnet build "ContestService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ContestService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ContestService.dll"]