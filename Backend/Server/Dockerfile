FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 7000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Server/Server.csproj", "./"]
COPY ["./Domain/", "../Domain/"]
COPY ["./TelegramBot/", "../TelegramBot/"]
RUN dotnet restore
COPY . .

WORKDIR /src/TelegramBot
RUN dotnet build -c Release -o /app

WORKDIR /src/Domain
RUN dotnet build -c Release -o /app

WORKDIR /src/Server
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install -g dotnet-ef --version 6.0.1
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Server.dll"]