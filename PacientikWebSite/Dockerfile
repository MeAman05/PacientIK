FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish PacientikWebSite.csproj -c Release -o /app/publish

FROM nginx:alpine
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
# этот файл должен тоже лежать в PacientikWebSite/
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
