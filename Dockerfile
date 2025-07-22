# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia o .csproj e restaura
COPY Src/CurrencyConverterBackend/*.csproj ./CurrencyConverterBackend/
RUN dotnet restore ./CurrencyConverterBackend/CurrencyConverterBackend.csproj

# Copia o resto e publica
COPY Src ./Src
RUN dotnet publish ./Src/CurrencyConverterBackend/CurrencyConverterBackend.csproj -c Release -o /publish

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "CurrencyConverterBackend.dll"]
