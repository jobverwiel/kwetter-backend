FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["AuthenticationService.csproj", "AuthenticationService/"]
RUN dotnet restore "AuthenticationService/AuthenticationService.csproj"
WORKDIR "/src/AuthenticationService"
COPY . .
RUN dotnet build "AuthenticationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthenticationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthenticationService.dll"]