FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EnergyMixApi/EnergyMixApi/EnergyMixApi.csproj", "EnergyMixApi/EnergyMixApi/"]
RUN dotnet restore "EnergyMixApi/EnergyMixApi/EnergyMixApi.csproj"
COPY . .
WORKDIR "/src/EnergyMixApi/EnergyMixApi"
RUN dotnet build "EnergyMixApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EnergyMixApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnergyMixApi.dll"]