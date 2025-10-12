FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SubmissionService/SubmissionService.csproj", "SubmissionService/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "SubmissionService/SubmissionService.csproj"
COPY . .
WORKDIR "/src/SubmissionService"
RUN dotnet build "SubmissionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SubmissionService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SubmissionService.dll"]