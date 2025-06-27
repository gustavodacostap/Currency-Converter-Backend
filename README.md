# 💱 Currency Converter Backend

Este é o backend de um conversor de moedas em tempo real, desenvolvido com **ASP.NET Core 9**, que consome uma API externa de câmbio para retornar valores convertidos entre duas moedas.

---

## 🚀 Funcionalidades

- API RESTful para conversão de moedas
- Consumo de API externa (`freecurrencyapi.com`)
- Cálculo e resposta com valor convertido
- Suporte a parâmetros via query string (`from`, `to`, `amount`)
- Swagger UI para teste dos endpoints

---

## 🧰 Tecnologias Utilizadas

- ASP.NET Core 9 (Web API)
- C#
- `HttpClient` (para consumir API externa)
- Injeção de dependência (`IOptions<T>`)
- Swagger (`Swashbuckle.AspNetCore`)
- Arquivo de configuração via `appsettings.json`