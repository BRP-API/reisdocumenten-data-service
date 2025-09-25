# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
# Disable config change monitoring for better container performance
# See https://github.com/Contrast-Security-OSS/agent-operator/blob/master/docs/public/08-tips.md
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
WORKDIR /app

# This stage is used to build the service project
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src

COPY ["Reisdocumenten.Data.Service/Reisdocumenten.Data.Service.csproj", "Reisdocumenten.Data.Service/"]
COPY ["Rvig.Base.App/Rvig.Base.App.csproj", "Rvig.Base.App/"]
COPY ["Brp.Shared.Infrastructure/Brp.Shared.Infrastructure.csproj", "Brp.Shared.Infrastructure/"]
COPY ["Serilog.Sinks.PersistentFile/Serilog.Sinks.PersistentFile.csproj", "Serilog.Sinks.PersistentFile/"]
COPY ["Rvig.Data.Base.WebApi/Rvig.Data.Base.WebApi.csproj", "Rvig.Data.Base.WebApi/"]
COPY ["Rvig.Data.Base/Rvig.Data.Base.csproj", "Rvig.Data.Base/"]
COPY ["Rvig.BrpApi.Shared/Rvig.BrpApi.Shared.csproj", "Rvig.BrpApi.Shared/"]
COPY ["Rvig.Data.Base.Postgres/Rvig.Data.Base.Postgres.csproj", "Rvig.Data.Base.Postgres/"]
COPY ["Rvig.Data.Reisdocumenten/Rvig.Data.Reisdocumenten.csproj", "Rvig.Data.Reisdocumenten/"]
COPY ["Rvig.BrpApi.Reisdocumenten/Rvig.BrpApi.Reisdocumenten.csproj", "Rvig.BrpApi.Reisdocumenten/"]

RUN dotnet restore "./Reisdocumenten.Data.Service/Reisdocumenten.Data.Service.csproj"

COPY . .

WORKDIR "/src/Reisdocumenten.Data.Service"
RUN dotnet build "./Reisdocumenten.Data.Service.csproj" -c "$BUILD_CONFIGURATION" -o /app/build --arch "$TARGETARCH"

# This stage is used to publish the service project to be copied to the final stage
FROM --platform=$BUILDPLATFORM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
RUN dotnet publish "./Reisdocumenten.Data.Service.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false --arch "$TARGETARCH"

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN addgroup -g 1000 -S brp-api && \
    adduser -u 1000 -S -D -g brp-api brp-api-user && \
    chown -R brp-api-user:brp-api /app

# Set user for running the application
USER brp-api-user

ENTRYPOINT ["dotnet", "Reisdocumenten.Data.Service.dll"]