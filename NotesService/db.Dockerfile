FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY NotesService.Api/*.csproj NotesService.Api/
COPY NotesService.Core/*.csproj NotesService.Core/
COPY NotesService.DataAccess/*.csproj NotesService.DataAccess/
COPY NotesService.UnitTests/*.csproj NotesService.UnitTests/
RUN dotnet restore

COPY .config ./
RUN dotnet tool restore

# Copy everything else and generate SQL script from migrations
COPY . ./
RUN dotnet ef migrations script -p NotesService.DataAccess -s NotesService.Api -o init-db.sql

# Build runtime image
FROM postgres:12.0
WORKDIR /docker-entrypoint-initdb.d
ENV POSTGRES_PASSWORD Pass@word
COPY --from=build-env /app/init-db.sql .
