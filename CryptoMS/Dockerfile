FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CryptoMS/CryptoMS.csproj", "CryptoMS/"]
RUN dotnet restore "CryptoMS/CryptoMS.csproj"
COPY . .
WORKDIR "/src/CryptoMS"
RUN dotnet build "CryptoMS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoMS.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoMS.dll"]
