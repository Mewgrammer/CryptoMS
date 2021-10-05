FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RegistrationAuthority/RegistrationAuthority.csproj", "RegistrationAuthority/"]
RUN dotnet restore "RegistrationAuthority/RegistrationAuthority.csproj"
COPY . .
WORKDIR "/src/RegistrationAuthority"
RUN dotnet build "RegistrationAuthority.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RegistrationAuthority.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RegistrationAuthority.dll"]
