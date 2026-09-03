FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# копируем всё решение
COPY . ./

# публикуем именно проект из подпапки
RUN dotnet restore PacientikWebSite/PacientikWebSite.csproj
RUN dotnet publish PacientikWebSite/PacientikWebSite.csproj -c Release -o /app/publish --no-restore /p:UseAppHost=false

RUN ls -lh /app/publish/wwwroot/_framework/ | head -30
RUN ls -lh /app/publish/wwwroot/ | head -30

FROM nginx:alpine
# чистим дефолтный конфиг nginx чтобы не было duplicate wasm
RUN rm /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot/ /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
