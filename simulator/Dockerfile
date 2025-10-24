# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish simulator.csproj -c Release -o /app

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app

# 🔹 Instalar netcat
RUN apt-get update && apt-get install -y netcat-openbsd

COPY --from=build /app .
COPY wait-for-rabbit.sh /wait-for-rabbit.sh
RUN chmod +x /wait-for-rabbit.sh
ENTRYPOINT ["/wait-for-rabbit.sh", "rabbitmq", "dotnet", "simulator.dll"]
