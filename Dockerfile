FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY WashFlow.Api.csproj ./
RUN dotnet restore ./WashFlow.Api.csproj

COPY . .
RUN dotnet publish ./WashFlow.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WashFlow.Api.dll"]
