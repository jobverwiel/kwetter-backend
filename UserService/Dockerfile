#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 22726/tcp

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["UserService.csproj", "UserService/"]
RUN dotnet restore "UserService/UserService.csproj"
WORKDIR "/src/UserService"
COPY . .
RUN dotnet build "UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.dll"]