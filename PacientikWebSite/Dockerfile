FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PacientikWebSite.csproj ./
RUN dotnet restore PacientikWebSite.csproj

COPY . ./

RUN dotnet publish PacientikWebSite.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

# === FIX дл€ твоего бага ===
# ¬ырезаем #[.{fingerprint}] из index.html если SDK его оставил
RUN sed -i 's/#\[.{fingerprint}\]//g' /app/publish/wwwroot/index.html && \
    sed -i 's/\?v=\[.{fingerprint}\]//g' /app/publish/wwwroot/index.html && \
    ls -l /app/publish/wwwroot/_framework/ | head -20 && \
    cat /app/publish/wwwroot/index.html | grep -o "_framework/[^\"']*"

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot/ /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
