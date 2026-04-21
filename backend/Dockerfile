# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS builder
WORKDIR /app

COPY src/ClearErp.Domain/ClearErp.Domain.csproj src/ClearErp.Domain/
COPY src/ClearErp.Application/ClearErp.Application.csproj src/ClearErp.Application/
COPY src/ClearErp.Infrastructure/ClearErp.Infrastructure.csproj src/ClearErp.Infrastructure/
COPY src/ClearErp.Api/ClearErp.Api.csproj src/ClearErp.Api/

RUN dotnet restore src/ClearErp.Api/ClearErp.Api.csproj

COPY ./src ./src

RUN dotnet publish src/ClearErp.Api/ClearErp.Api.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=builder /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000
CMD ["dotnet", "ClearErp.Api.dll"]
