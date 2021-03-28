#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OcelotGateway.csproj", "OcelotGateway/"]
RUN dotnet restore "OcelotGateway/OcelotGateway.csproj" 

WORKDIR "/src/OcelotGateway"
COPY . .

RUN dotnet build "OcelotGateway.csproj" -c Release -o /app/build
COPY . ../


FROM build AS publish
RUN dotnet publish "OcelotGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80/tcp
ENV ASPNETCORE_URLS http://*:57929


ENTRYPOINT ["dotnet", "OcelotGateway.dll"]