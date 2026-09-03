FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN find . -name "*.csproj" -type f
RUN dotnet publish ./PacientIK/PacientikWebSite/PacientikWebSite.csproj -c Release -o /app/publish /p:BlazorEnableCompression=false /p:PublishTrimmed=false

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY --from=build /src/PacientIK/PacientikWebSite/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
