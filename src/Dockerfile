FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ByReplace.sln
RUN dotnet build ByReplace.sln -c Release -o /app
RUN dotnet test ByReplace.sln

# Build runtime image
#FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
#WORKDIR /app
#COPY --from=build /app .
#ENTRYPOINT ["dotnet", "ByReplace.dll"]
#