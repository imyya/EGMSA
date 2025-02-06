# Étape 1 : Build de l’application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier les fichiers du projet
COPY . ./
RUN dotnet publish -c Release -o out

# Étape 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copier les fichiers buildés
COPY --from=build /app/out ./

# Exposer le port utilisé par Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Commande de démarrage
CMD ["dotnet", "EMGMSA.dll"]
