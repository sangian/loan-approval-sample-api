FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LoanApproval.WebApi/LoanApproval.WebApi.csproj", "LoanApproval.WebApi/"]
RUN dotnet restore "LoanApproval.WebApi/LoanApproval.WebApi.csproj"
COPY . .
WORKDIR "/src/LoanApproval.WebApi"
RUN dotnet build "./LoanApproval.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LoanApproval.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoanApproval.WebApi.dll"]
