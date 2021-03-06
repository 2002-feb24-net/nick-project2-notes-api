FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln ./
COPY NotesService.Api/*.csproj NotesService.Api/
COPY NotesService.Core/*.csproj NotesService.Core/
COPY NotesService.DataAccess/*.csproj NotesService.DataAccess/
COPY NotesService.UnitTests/*.csproj NotesService.UnitTests/
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish NotesService.Api -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NotesService.Api.dll"]
