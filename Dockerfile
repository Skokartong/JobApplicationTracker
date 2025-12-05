FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY JobApplicationTracker.sln .
COPY JobApplicationTracker/JobApplicationTracker.csproj JobApplicationTracker/

RUN dotnet restore JobApplicationTracker/JobApplicationTracker.csproj

COPY . .

WORKDIR /src/JobApplicationTracker
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobApplicationTracker.dll"]
