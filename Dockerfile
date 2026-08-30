FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore PacientIK/PacientIK.csproj
RUN dotnet publish PacientIK/PacientIK.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet PacientIK.dll
