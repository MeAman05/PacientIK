FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем проект и восстанавливаем зависимости
COPY PacientikWebSite.csproj ./
RUN dotnet restore

# Копируем остальные файлы и публикуем
COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Финальный образ с Nginx
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Копируем собранный Blazor
COPY --from=build /app/publish/wwwroot .

# Копируем конфиг Nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Удаляем дефолтный конфиг
RUN rm /etc/nginx/conf.d/default.conf 2>/dev/null || true

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
