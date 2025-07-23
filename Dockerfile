# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copia o .csproj e restaura as dependências
COPY CurrencyConverter.Backend.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos
COPY . ./
RUN dotnet publish -c Release -o /publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

ENTRYPOINT ["dotnet", "CurrencyConverter.Backend.dll"]
