
# Finance Tracker API

[.NET 8 SDK](httpsdotnet.microsoft.comen-usdownloaddotnet8.0)  [Clean Architecture](httpscleanarchitecture.com)  [CQRS](httpsmartinfowler.comblikiCQRS.html)

## Architecture Overview
This API follows Clean Architecture principles with explicit separation of concerns

``` plaintext
src
├── FinanceTracker.Domain          # Core business logic
├── FinanceTracker.Application     # Use cases and application services
├── FinanceTracker.Infrastructure  # Database access and external integrations
├── FinanceTracker.Api             # HTTP controllers and API surface
```

## Folder Structure
```plaintext
src
├── FinanceTracker.Domain
│   ├── Models                     # Core entities (Expense, Category)
│   ├── Enums                      # Value objects (PaymentMethod)
├── FinanceTracker.Application
│   ├── Commands                   # CQRS commands (CreateExpenseCommand)
│   ├── Queries                    # CQRS queries (GetExpensesQuery)
│   ├── Handlers                   # Commandquery handlers
│   ├── Validators                 # FluentValidation rules
│   ├── DTOs                       # Data transfer objects
│   ├── Repositories               # Repository interfaces
├── FinanceTracker.Infrastructure
│   ├── DbContexts                 # EF Core database context
│   ├── Repositories               # Repository implementations
│   ├── Mappings                   # AutoMapper profiles
├── FinanceTracker.Api
│   ├── Controllers                # HTTP endpoints
│   ├── Endpoints                  # Minimal API endpoints (optional)
```

## Key Features
### CQRS Implementation
- Separate command (write) and query (read) pipelines
- FluentValidation for input validation
- FluentResults for consistent error handling

### Domain-Driven Design
- Aggregate root entities (Expense)
- Value objects (PaymentMethod)
- Encapsulated business logic

### Infrastructure Abstraction
- Repository pattern with generics
- Unit of work pattern
- Database-agnostic design

## Models
### Expense.cs
```csharp
public class Expense  AggregateRoot
{
    public DateTime Date { get; set; }
    public string Notes { get; set; }
    public decimal TotalAmount = ExpenseItems.Sum(i = i.Total);
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollectionExpenseItem ExpenseItems { get; set; }
}
```

### Category.cs
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ParentCategoryId { get; set; }
    public virtual Category ParentCategory { get; set; }
    public virtual ICollectionCategory SubCategories { get; set; }
}
```

## Repository Interface
```csharp
public interface IRepositoryT where T  class
{
    TaskT GetByIdAsync(int id, CancellationToken cancellationToken = default);
    TaskListT GetAllAsync(
        ExpressionFuncT, bool filter = null,
        FuncIQueryableT, IOrderedQueryableT orderBy = null,
        ListFuncIQueryableT, IIncludableQueryableT, object includes = null,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
```

## Getting Started
### Prerequisites
- .NET 8 SDK
- SQL Server (or compatible database)
- Visual Studio 2022+ or VS Code

### Installation
#### Clone the repository
```bash
git clone repo-url
```

#### Restore dependencies
```bash
dotnet restore
```

#### Configure connection string in `appsettings.json`

#### Run migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Run the API
```bash
dotnet run --project srcFinanceTracker.ApiFinanceTracker.Api.csproj
```

## API Endpoints
### Create Expense
```http
POST apiexpenses
Content-Type applicationjson

{
    date 2024-05-15,
    notes Grocery shopping,
    items [
        {
            categoryId 1,
            paymentMethod Cash,
            description Milk,
            price 5.99,
            quantity 2
        }
    ]
}
```

### Get Expenses (Paginated)
```http
GET apiexpensespage=1&pageSize=10&startDate=2024-01-01&endDate=2024-12-31
```

## Validation & Error Handling
All endpoints use FluentValidation and return structured errors via FluentResults
```json
{
    errors {
        items[0].categoryId [Category not found],
        items[0].price [Price must be greater than 0]
    },
    message Validation failed
}
```

## Advanced Features
### Pagination
- Automatic pagination with metadata
- Customizable page size and number

### Eager Loading
- Support for multiple include paths
- Explicit loading of related entities

### Localization
- FluentValidation error messages localization
- Support for multiple cultures

## Malaysian Context Considerations
### Payment Methods
- Includes local methods like DuitNow, Boost, and TNG eWallet

### Tax Compliance
- Fields for SSTGST tracking
- Business expense categorization

### Currency Support
- Multi-currency transactions
- Currency conversion rates

## Testing
```bash
dotnet test --project testsFinanceTracker.TestsFinanceTracker.Tests.csproj
```

## Contributing
1. Fork the repository
2. Create a feature branch
3. Commit changes with descriptive messages
4. Submit a pull request

## License
MIT License
```

