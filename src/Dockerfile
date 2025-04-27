FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ByReplace.sln
RUN dotnet build ByReplace.sln -c Release -o /app
RUN dotnet test ByReplace.sln

