FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PacientikWebSite.csproj ./
RUN dotnet restore PacientikWebSite.csproj

COPY . ./
RUN dotnet publish PacientikWebSite.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# ”ƒјЋя≈ћ сжатые файлы чтобы не ломать SRI проверку
RUN rm -f /app/publish/wwwroot/_framework/*.gz \
          /app/publish/wwwroot/_framework/*.br \
          /app/publish/wwwroot/_framework/*.gz.* \
          /app/publish/wwwroot/*.gz \
          /app/publish/wwwroot/*.br && \
    sed -i 's/#\[.{fingerprint}\]//g' /app/publish/wwwroot/index.html || true

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot/ /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
