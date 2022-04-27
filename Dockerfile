FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OpenTelemetrySample/OpenTelemetrySample.csproj", "OpenTelemetrySample/"]
RUN dotnet restore "OpenTelemetrySample/OpenTelemetrySample.csproj"
COPY . .
WORKDIR "/src/OpenTelemetrySample"
RUN dotnet build "OpenTelemetrySample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenTelemetrySample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenTelemetrySample.dll"]