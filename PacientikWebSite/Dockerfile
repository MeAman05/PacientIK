FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore PacientikWebSite.csproj

RUN dotnet publish PacientikWebSite.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM nginx:alpine

COPY --from=build /app/publish/wwwroot/ /usr/share/nginx/html/

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
