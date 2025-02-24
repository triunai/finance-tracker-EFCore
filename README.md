markdown
# 💰 Finance Tracker API 📊

[🛠️ .NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) | [🏗️ Clean Architecture](https://cleanarchitecture.com/) | [🔄 CQRS](https://martinfowler.com/bliki/CQRS.html)

## 🏛️ Architecture Overview
This API follows **🧼 Clean Architecture** principles with explicit separation of concerns:

```plaintext
src/
├── 📂 FinanceTracker.Domain/          # 🧠 Core business logic
├── 📂 FinanceTracker.Application/     # 📜 Use cases & services
├── 📂 FinanceTracker.Infrastructure/  # 🗄️ Database & integrations
├── 📂 FinanceTracker.Api/             # 🌐 API controllers
```

## 📂 Folder Structure
```plaintext
src/
├── 📂 FinanceTracker.Domain/
│   ├── 📂 Models/                     # 🏦 Entities (💸 Expense, 📌 Category)
│   ├── 📂 Enums/                      # 🔢 Value objects (💳 PaymentMethod)
├── 📂 FinanceTracker.Application/
│   ├── 📂 Commands/                   # 🚀 CQRS Commands (➕ CreateExpense)
│   ├── 📂 Queries/                    # 🔍 CQRS Queries (📊 GetExpenses)
│   ├── 📂 Handlers/                   # 🎮 Command/query handlers
│   ├── 📂 Validators/                 # ✅ FluentValidation rules
│   ├── 📂 DTOs/                       # 📩 Data transfer objects
│   ├── 📂 Repositories/               # 🗂️ Repository interfaces
├── 📂 FinanceTracker.Infrastructure/
│   ├── 📂 DbContexts/                 # 🏗️ EF Core database
│   ├── 📂 Repositories/               # 🔄 Repository implementations
│   ├── 📂 Mappings/                   # 🔁 AutoMapper profiles
├── 📂 FinanceTracker.Api/
│   ├── 📂 Controllers/                # 🌍 HTTP endpoints
│   ├── 📂 Endpoints/                  # 🚦 Minimal API endpoints
```

## 🔑 Key Features
### 🔄 CQRS Implementation
- ✍️ **Command (write)** & 🔍 **Query (read)** separation
- ✅ **FluentValidation** for 🛂 input validation
- ⚠️ **FluentResults** for 🎯 error handling

### 🏗️ Domain-Driven Design
- 📦 **Aggregate root entities** (💸 Expense)
- 🔢 **Value objects** (💳 PaymentMethod)
- 🧠 **Encapsulated business logic**

### 🗄️ Infrastructure Abstraction
- 📁 **Repository pattern** w/ generics
- 🔄 **Unit of work pattern**
- 📡 **Database-agnostic design**

## 📌 Models
### 💸 Expense.cs
```csharp
public class Expense : AggregateRoot
{
    public DateTime Date { get; set; }
    public string Notes { get; set; }
    public decimal TotalAmount => ExpenseItems.Sum(i => i.Total);
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual ICollection<ExpenseItem> ExpenseItems { get; set; }
}
```

### 📌 Category.cs
```csharp
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
    public virtual Category ParentCategory { get; set; }
    public virtual ICollection<Category> SubCategories { get; set; }
}
```

## 🗂️ Repository Interface
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        List<Func<IQueryable<T>, IIncludableQueryable<T, object>>>? includes = null,
        CancellationToken cancellationToken = default
    );
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
```

## 🚀 Getting Started
### 🛠️ Prerequisites
- 🔧 **.NET 8 SDK**
- 🗄️ **SQL Server** (or compatible)
- 💻 **Visual Studio 2022+ or VS Code**

### 🏗️ Installation
#### 📥 Clone the repository
```bash
git clone <repo-url>
```
#### 📦 Restore dependencies
```bash
dotnet restore
```
#### ⚙️ Configure connection string in `appsettings.json`
#### 🏗️ Run migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
#### 🚀 Run the API
```bash
dotnet run --project src/FinanceTracker.Api/FinanceTracker.Api.csproj
```

## 🔗 API Endpoints
### ➕ Create Expense
```http
POST /api/expenses
Content-Type: application/json

{
    "date": "2024-05-15",
    "notes": "🛒 Grocery shopping",
    "items": [
        {
            "categoryId": 1,
            "paymentMethod": "💵 Cash",
            "description": "🥛 Milk",
            "price": 5.99,
            "quantity": 2
        }
    ]
}
```
### 📊 Get Expenses (Paginated)
```http
GET /api/expenses?page=1&pageSize=10&startDate=2024-01-01&endDate=2024-12-31
```

## ❌ Validation & Error Handling
All endpoints use **✅ FluentValidation** and return structured errors via **⚠️ FluentResults**:
```json
{
    "errors": {
        "items[0].categoryId": ["🚫 Category not found"],
        "items[0].price": ["🚫 Price must be greater than 0"]
    },
    "message": "❌ Validation failed"
}
```

## 🎯 Advanced Features
### 📌 Pagination
- 📖 **Automatic pagination** w/ metadata
- 🔢 **Customizable page size & number**

### 🔄 Eager Loading
- 📂 **Multiple include paths**
- 👀 **Explicit entity loading**

### 🌍 Localization
- 🌐 **Localized error messages**
- 🏴 **Multi-language support**

## 🇲🇾 Malaysian Context
### 💳 Payment Methods
- ✅ Supports **DuitNow, Boost, & TNG eWallet**

### 📜 Tax Compliance
- 💰 **SST/GST tracking**
- 📊 **Business expense categorization**

### 💱 Currency Support
- 💵 **Multi-currency transactions**
- 🔄 **Exchange rate conversion**

## 🧪 Testing
```bash
dotnet test --project tests/FinanceTracker.Tests/FinanceTracker.Tests.csproj
```

## 🤝 Contributing
1. 🍴 **Fork the repository**
2. 🌱 **Create a feature branch**
3. 📝 **Commit changes w/ descriptions**
4. 🔄 **Submit a pull request**

## 📜 License
🔓 **MIT License**


