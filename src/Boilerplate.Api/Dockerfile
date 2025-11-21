FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Boilerplate.Api/Boilerplate.Api.csproj", "src/Boilerplate.Api/"]
COPY ["src/Boilerplate.Application/Boilerplate.Application.csproj", "src/Boilerplate.Application/"]
COPY ["src/Boilerplate.Domain/Boilerplate.Domain.csproj", "src/Boilerplate.Domain/"]
COPY ["src/Boilerplate.Infrastructure/Boilerplate.Infrastructure.csproj", "src/Boilerplate.Infrastructure/"]
RUN dotnet restore "./src/Boilerplate.Api/Boilerplate.Api.csproj"
COPY . .
WORKDIR "/src/src/Boilerplate.Api"
RUN dotnet build "./Boilerplate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Boilerplate.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Boilerplate.Api.dll"]