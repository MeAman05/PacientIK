FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY PacientIK/PacientikWebSite/PacientikWebSite.csproj PacientIK/PacientikWebSite/
RUN dotnet restore PacientIK/PacientikWebSite/PacientikWebSite.csproj
COPY . .
RUN dotnet publish PacientIK/PacientikWebSite/PacientikWebSite.csproj -c Release -o /app/publish

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY PacientIK/PacientikWebSite/nginx.conf /etc/nginx/conf.d/default.conf
