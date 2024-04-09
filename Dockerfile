# Utilizar la imagen oficial de .NET Core como base
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Establecer el directorio de trabajo dentro del contenedor
WORKDIR /app

# Copiar el archivo csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar el resto de los archivos y compilar la aplicación
COPY . ./
RUN dotnet publish -c Release -o out

# Crear la imagen final
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NombreDeTuApp.dll"]
