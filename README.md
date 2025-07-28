# 💱 Exchange Rate Offers API

This project is a .NET 8 Web API that compares remittance exchange rates from multiple external providers and returns the best available offer. It follows Clean Architecture and includes logging, validation, error handling, and Swagger documentation.

---

## ✅ Prerequisites

Before running the project, ensure you have the following installed:

- ✅ **.NET 8 SDK** (if running locally):  
  [Install .NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

- ✅ **Docker** (if running in a container):  
  [Install Docker](https://www.docker.com/products/docker-desktop)  
  Docker must be installed and running.

---

## 🚀 Features

- Accepts a source currency, target currency, and amount.
- Queries multiple external exchange rate APIs.
- Selects and returns the best (highest) converted offer.
- Returns validation errors as structured `ProblemDetails`.
- Fault-tolerant: skips APIs that fail and continues with the rest.
- Exposes a single endpoint: `POST /exchange-rate/compare`.
- Integrated Swagger UI for testing and exploration.

---

## 🧾 Request/Response

### Request: `POST /exchange-rate/compare`

```json
{
  "sourceCurrency": "USD",
  "targetCurrency": "EUR",
  "amount": 100
}
```

### Successful Response: `200 OK`

```json
{
  "provider": "ErApiClient",
  "convertedAmount": 92.50
}
```

### Validation Error: `400 Bad Request`

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Validation failed.",
  "status": 400,
  "detail": "One or more validation errors ocurred.",
  "errors": {
    "SourceCurrency": ["Source currency code is not valid."],
    "TargetCurrency": ["Target currency code is not valid."]
  }
}
```

---

## 🧪 Running the API

### Option 1: Run Locally (CLI)

1. Restore and build:

   ```bash
   dotnet restore
   dotnet build
   ```

2. Run the API:

   ```bash
   dotnet run --project src/ExchangeRateOffers.Api
   ```

3. Visit [https://localhost:5001/swagger](https://localhost:5001/swagger) or [http://localhost:5000/swagger](http://localhost:5000/swagger)

---

### Option 2: Run in Docker

1. Build the image:

   ```bash
   docker build -t exchange-rate-api .
   ```

2. Run the container:

   ```bash
   docker run -d -p 8099:8080 --name exchange-rate-api exchange-rate-api
   ```

3. Visit Swagger UI:

   [http://localhost:8099/swagger/index.html](http://localhost:8099/swagger/index.html)

---

## 🧪 Running Tests

### Locally:

```bash
dotnet test
```

Make sure you run it from the solution root (where the `.sln` is located).

---

## 🌍 External APIs Used

- [ER API](https://open.er-api.com)
- [Fawaz Currency API](https://github.com/fawazahmed0/currency-api)
- [Frankfurter API](https://www.frankfurter.app/)

---

## 🛠️ Technologies

- .NET 8
- ASP.NET Core Web API
- Clean Architecture
- FluentValidation
- Swagger / Swashbuckle
- Serilog logging
- Custom exception handling middleware
- Docker support

---

## 📁 Project Structure

```
src/
├── ExchangeRateOffers.Api/        # API project
├── Application/                   # Application services and interfaces
├── Domain/                        # Domain models and constants
├── Infrastructure/                # API clients and providers
tests/
├── ExchangeRateOffers.Tests/     # Unit tests
```

---

## 📬 Contact

If you have questions or suggestions, feel free to reach out or open an issue.

---

## 📝 License

MIT License (optional - add a LICENSE file if needed)
