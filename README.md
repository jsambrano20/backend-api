# Developer Evaluation Project

`READ CAREFULLY`

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

## Overview
This section provides a high-level overview of the project and the various skills and competencies it aims to assess for developer candidates. 

See [Overview](/.doc/overview.md)

## Tech Stack
This section lists the key technologies used in the project, including the backend, testing, frontend, and database components. 

See [Tech Stack](/.doc/tech-stack.md)

## Frameworks
This section outlines the frameworks and libraries that are leveraged in the project to enhance development productivity and maintainability. 

See [Frameworks](/.doc/frameworks.md)

<!-- 
## API Structure
This section includes links to the detailed documentation for the different API resources:
- [API General](./docs/general-api.md)
- [Products API](/.doc/products-api.md)
- [Carts API](/.doc/carts-api.md)
- [Users API](/.doc/users-api.md)
- [Auth API](/.doc/auth-api.md)
-->

## Project Structure
This section describes the overall structure and organization of the project files and directories. 

See [Project Structure](/.doc/project-structure.md)

## How to Configure, Run and Test

The implementation lives in `template/backend` (.NET 8, ASP.NET Core Web API, EF Core + PostgreSQL).

### Prerequisites

* .NET 8 SDK
* Docker (to run PostgreSQL), or a local PostgreSQL instance

### 1. Start the database

```bash
cd template/backend
docker run -d --name ambev_eval_pg \
  -e POSTGRES_DB=developer_evaluation \
  -e POSTGRES_USER=developer \
  -e POSTGRES_PASSWORD=ev@luAt10n \
  -p 5432:5432 postgres:13
```

The connection string is already set in `src/Ambev.DeveloperEvaluation.WebApi/appsettings.json`
(`ConnectionStrings:DefaultConnection`). Adjust it if you point to a different PostgreSQL instance.

### 2. Apply database migrations

```bash
dotnet tool install --global dotnet-ef   # if not installed yet
dotnet ef database update \
  --project src/Ambev.DeveloperEvaluation.ORM \
  --startup-project src/Ambev.DeveloperEvaluation.WebApi
```

### 3. Run the API

```bash
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

Swagger UI is available at `/swagger` in the Development environment. The Sales endpoints are
exposed under `api/Sales` (Create, Get by id, List with pagination/filtering/ordering, Update,
Delete, Cancel sale, Cancel item), implementing the full CRUD plus the cancellation flows described
in the use case above. Discount/quantity business rules are enforced in the domain layer
(`Ambev.DeveloperEvaluation.Domain.Services.SaleDiscountCalculator`) and `SaleCreated` /
`SaleModified` / `SaleCancelled` / `ItemCancelled` events are logged via `ILogger` in the
corresponding command handlers (no message broker is required).

### 4. Run the tests

```bash
dotnet test tests/Ambev.DeveloperEvaluation.Unit/Ambev.DeveloperEvaluation.Unit.csproj
```

Unit tests cover the discount business rules (`SaleDiscountCalculatorTests`), the `Sale` entity
behavior (`SaleTests`) and the CQRS handlers (`CreateSaleHandlerTests`, `CancelSaleHandlerTests`).

> Note: this machine had a private NuGet feed registered globally that returns 401 Unauthorized.
> A `NuGet.Config` restricting restore to `nuget.org` was added at `template/backend/NuGet.Config`
> so `dotnet build`/`dotnet test` work out of the box regardless of machine-level NuGet sources.
