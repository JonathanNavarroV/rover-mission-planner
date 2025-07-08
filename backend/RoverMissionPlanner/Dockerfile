# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY RoverMissionPlanner.API/*.csproj ./RoverMissionPlanner.API/
RUN dotnet restore RoverMissionPlanner.API/RoverMissionPlanner.API.csproj

# Copiar el resto de archivos y compilar
COPY . .
RUN dotnet publish RoverMissionPlanner.API/RoverMissionPlanner.API.csproj -c Release -o out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Configurar variable para que escuche HTTP en el puerto 5225 (sin HTTPS)
ENV ASPNETCORE_URLS=http://+:5225

# Exponer el puerto 5225
EXPOSE 5225

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "RoverMissionPlanner.API.dll"]
