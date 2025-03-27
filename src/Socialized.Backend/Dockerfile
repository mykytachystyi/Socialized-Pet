FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

EXPOSE 8000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Socialized.Core/Socialized.Core.csproj", "Socialized.Core/Socialized.Core.csproj"]
COPY ["Socialized.Domain/Socialized.Domain.csproj", "Socialized.Domain/Socialized.Domain.csproj"]
COPY ["Socialized.Infrastructure/Socialized.Infrastructure.csproj", "Socialized.Infrastructure/Socialized.Infrastructure.csproj"]
COPY ["Socialized.UseCases/Socialized.UseCases.csproj", "Socialized.UseCases/Socialized.UseCases.csproj"]
COPY ["Socialized.API/Socialized.API.csproj", "Socialized.API/Socialized.API.csproj"]
RUN dotnet restore "Socialized.API/Socialized.API.csproj"
COPY . ./
WORKDIR "/src/Socialized.API"
RUN dotnet build "Socialized.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Socialized.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Socialized.API.dll"]