FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProblemService/ProblemService.csproj", "ProblemService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "ProblemService/ProblemService.csproj"
COPY . .
WORKDIR "/src/ProblemService"
RUN dotnet build "ProblemService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProblemService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProblemService.dll"]