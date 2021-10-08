FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CertificateAuthority/CertificateAuthority.csproj", "CertificateAuthority/"]
RUN dotnet restore "CertificateAuthority/CertificateAuthority.csproj"
COPY . .
WORKDIR "/src/CertificateAuthority"
RUN dotnet build "CertificateAuthority.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CertificateAuthority.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CertificateAuthority.dll"]
