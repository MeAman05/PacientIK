FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY PacientikWebSite.csproj ./
RUN dotnet restore PacientikWebSite.csproj

COPY . ./
RUN dotnet publish PacientikWebSite.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

# Copy the static assets from the build stage
COPY --from=build /app/publish/wwwroot .

# Copy custom Nginx configuration for Blazor WebAssembly
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Remove default nginx config to avoid conflicts
RUN rm /etc/nginx/conf.d/default.conf 2>/dev/null || true

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
