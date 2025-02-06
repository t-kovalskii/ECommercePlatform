FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ECommercePlatform.Services.User.Web/ECommercePlatform.Services.User.Web.csproj", "ECommercePlatform.Services.User.Web/"]
RUN dotnet restore "ECommercePlatform.Services.User.Web/ECommercePlatform.Services.User.Web.csproj"
COPY . .
WORKDIR "/src/ECommercePlatform.Services.User.Web"
RUN dotnet build "ECommercePlatform.Services.User.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommercePlatform.Services.User.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommercePlatform.Services.User.Web.dll"]
