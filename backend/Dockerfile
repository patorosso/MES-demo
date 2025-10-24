FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish backend.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
RUN apt-get update && apt-get install -y netcat-openbsd
COPY --from=build /app .
COPY ../simulator/wait-for-rabbit.sh /wait-for-rabbit.sh
RUN chmod +x /wait-for-rabbit.sh
ENTRYPOINT ["/wait-for-rabbit.sh", "rabbitmq", "dotnet", "backend.dll"]
