FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
WORKDIR /src/PacientikWebSite
RUN dotnet publish PacientikWebSite.csproj -c Release -o /app/publish /p:BlazorEnableCompression=false /p:PublishTrimmed=false
RUN ls -lh /app/publish/wwwroot/_framework | head -n 100

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
COPY PacientikWebSite/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
