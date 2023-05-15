# BUILD
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ./src/FeestSpel ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# RUNNER IMAGE
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build /src/out .
WORKDIR /data
ENTRYPOINT ["dotnet", "/app/FeestSpel.dll"]